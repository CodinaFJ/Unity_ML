using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToGoalAgent : Agent
{
	[SerializeField] private Transform target;
	[SerializeField] private float goalReward;
	[SerializeField] private float wallPunishment;
	[SerializeField] private GameObject winMask;
	[SerializeField] private GameObject loseMask;

	private InputService 		inputService;
	private PlayerCollisions	playerCollisions;
	private PlayerMovement		playerMovement;

	protected void Start() 
	{
		inputService = ServiceLocator.Instance.GetService<InputService>();
		playerCollisions = GetComponent<PlayerCollisions>();
		playerMovement = GetComponent<PlayerMovement>();
		winMask.SetActive(false);
		loseMask.SetActive(false);

		RegisterCollisionsReactions();
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
		continuousAction[0] = inputService.Movement.x;

		ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
		discreteActions[0] = inputService.Jump > Mathf.Epsilon ? 1 : 0;
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		float moveHorizontal = actions.ContinuousActions[0];
		int jump = actions.DiscreteActions[0];

		playerMovement.Movement = new Vector2(moveHorizontal, playerMovement.Movement.y);
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
}
