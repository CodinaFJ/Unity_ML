using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
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
	private SpawnService 		spawnService;
	private PlayerCollisions	playerCollisions;
	private PlayerMovement		playerMovement;
	private float				episodeTimeElapsed;
	private float 				actionPunishmentReduction;
	private int 				contadorAcciones;

	public EnvironmentController	environmentController;

	protected void Start() 
	{
		inputService = ServiceLocator.Instance.GetService<InputService>();
		spawnService = ServiceLocator.Instance.GetService<SpawnService>();
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
		playerCollisions.CollisionWithPunishAction -= Punish;
		playerCollisions.TriggerWithPunishAction -= Punish;
	}

	public override void OnEpisodeBegin()
	{
		episodeTimeElapsed = 0;
		environmentController.BeginEpisode();//TODO: this should be done with an event
		contadorAcciones = 0;  // JM: variable de test para ver cuantas acciones se hacen de media hasta alcanzar el objetivo
		transform.position = spawnService.GetPositionInSpawnZone(this.gameObject);
		target.transform.position = spawnService.GetPositionInSpawnZone(target.gameObject);
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation((Vector2) this.transform.localPosition);
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
		contadorAcciones += 1;
		float moveRight = actions.ContinuousActions[0];
		float moveLeft = actions.ContinuousActions[1];
		int jump = actions.DiscreteActions[0];

		playerMovement.Movement = new Vector2(moveRight - moveLeft, playerMovement.Movement.y);
		if (jump == 1){
			playerMovement.Jump();
			AddReward(-0.1f); // Se da un pequeño punishment cada vez que salta para evitar que salte en exceso
		} 
		//this.LogInfo("Numero de acciones: " + contadorAcciones);
	}

	private void RegisterCollisionsReactions()
	{
		playerCollisions.CollisionWithPunishAction += Punish;
		playerCollisions.TriggerWithPunishAction += Punish;
	}

	private void Reward()
	{
		actionPunishmentReduction = episodeTimeElapsed*-0.03f;
		this.LogInfo("SUCCESS");
		AddReward(goalReward + actionPunishmentReduction); // Al valor de 10 por encontrar el objetivo le restamos un pequeño valor por cada accion realizada desde que empieza el episodio
		winMask.SetActive(true);
		loseMask.SetActive(false);
		this.LogInfo("Total Reward: " + GetCumulativeReward()); // JM: Test para ver cuanto reward estan consiguiendo
		EndEpisode();
	}

	private void Punish()
	{
		this.LogInfo("FAIL");
		SetReward(wallPunishment * -1);
		winMask.SetActive(false);
		loseMask.SetActive(true);
		this.LogInfo("Total Reward: " + GetCumulativeReward()); // JM: Test para ver cuanto reward estan consiguiendo
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
			this.LogInfo("Total Reward: " + GetCumulativeReward()); // JM: Test para ver cuanto reward estan consiguiendo
			EndEpisode();
		}
	}
}
