using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToGoalAgent : Agent
{
	[SerializeField] private Transform target;
	[SerializeField] private float goalReward;
	[SerializeField] private float wallPunishment;
	[SerializeField] private float finishEpisodePunishment = -1;
	[SerializeField] private GameObject winMask;
	[SerializeField] private GameObject loseMask;
	[SerializeField] private float episodeTimeLimit = 10;

	private InputService 		inputService;
	private PlayerCollisions	playerCollisions;
	private PlayerMovement		playerMovement;
	private float				episodeTimeElapsed;

	protected void Start() 
	{
		inputService = ServiceLocator.Instance.GetService<InputService>();
		playerCollisions = GetComponent<PlayerCollisions>();
		playerMovement = GetComponent<PlayerMovement>();
		winMask.SetActive(false);
		loseMask.SetActive(false);

		RegisterCollisionsReactions();
	}

	private void Update() 
	{
		CheckEndEpisode();	
	}

	private void CheckEndEpisode()
	{
		episodeTimeElapsed += Time.deltaTime;
		if (episodeTimeElapsed > episodeTimeLimit)
		{
			episodeTimeElapsed = 0;
			Reinforce(finishEpisodePunishment, true);
		}
	}

	protected override void OnDisable() 
	{
		base.OnDisable();
		playerCollisions.CollisionWithRewardAction -= Reward;
		playerCollisions.TriggerWithRewardAction -= Reward;
		playerCollisions.CollisionWithPunishAction -= Punish;
		playerCollisions.TriggerWithPunishAction -= Punish;
	}

	public override void OnEpisodeBegin()
	{
		transform.localPosition = new Vector2(Random.Range(-7f, 7f), Random.Range(-1f, 0f));
		target.transform.localPosition = new Vector2(Random.Range(-7.5f, 7.5f), Random.Range(-0.35f, -3f));
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation((Vector2) this.transform.localPosition);
		//sensor.AddObservation((Vector2) target.localPosition);
		foreach (var distance in GetComponent<PlayerRaycasts>().GetRayCollisionDistances())
			sensor.AddObservation(distance);
		foreach(var id in GetComponent<PlayerRaycasts>().GetRayCollisionIDs())
			sensor.AddObservation(id);
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
		continuousAction[0] = Mathf.Clamp(inputService.Movement.x, 0, 1);
		continuousAction[1] = Mathf.Abs(Mathf.Clamp(inputService.Movement.x, -1, 0));

		ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
		discreteActions[0] = inputService.Jump > Mathf.Epsilon ? 1 : 0;
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		float moveRight = actions.ContinuousActions[0];
		float moveLeft = actions.ContinuousActions[1];
		int jump = actions.DiscreteActions[0];

		playerMovement.Movement = new Vector2(moveRight - moveLeft, playerMovement.Movement.y);
		if (jump == 1) playerMovement.Jump();
	}

	private void RegisterCollisionsReactions()
	{
		playerCollisions.CollisionWithRewardAction += Reward;
		playerCollisions.TriggerWithRewardAction += Reward;
		playerCollisions.CollisionWithPunishAction += Punish;
		playerCollisions.TriggerWithPunishAction += Punish;
	}

	private void Reward()
	{
		this.LogInfo("SUCCESS");
		SetReward(goalReward);
		winMask.SetActive(true);
		loseMask.SetActive(false);
		EndEpisode();
	}

	private void Punish()
	{
		this.LogInfo("FAIL");
		SetReward(wallPunishment * -1);
		winMask.SetActive(false);
		loseMask.SetActive(true);
		EndEpisode();
	}

	public void Reinforce(float value, bool endEpisode)
	{
		SetReward(value);
		if (endEpisode)
		{
			bool win = value > 0;
			winMask.SetActive(win);
			loseMask.SetActive(!win);
			EndEpisode();
		}
	}
}
