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
		transform.localPosition = new Vector2(Random.Range(-7f, 0f), Random.Range(-3f, 3f));
		target.transform.localPosition = new Vector2(Random.Range(7f, 0f), Random.Range(-3f, 3f));
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation((Vector2) this.transform.localPosition);
		sensor.AddObservation((Vector2) target.localPosition);
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
		continuousAction[0] = inputService.Movement.x;
		continuousAction[1] = inputService.Movement.y;
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		float moveHorizontal = actions.ContinuousActions[0];
		float moveVertical = actions.ContinuousActions[1];

		playerMovement.Movement = new Vector2(moveHorizontal, moveVertical);
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
