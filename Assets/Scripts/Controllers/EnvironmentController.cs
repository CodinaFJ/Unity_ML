using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] GameObject	environmentGO;
	[SerializeField] GameObject	agentGO;

	List<Reward> rewardsOpenList;
	List<Reward> rewardsClosedList;

	MoveToGoalAgent agent;

	private SpawnService	spawnService;

	private void Start()
	{
		spawnService = ServiceLocator.Instance.GetService<SpawnService>();
		rewardsOpenList = environmentGO.GetComponentsInChildren<Reward>().ToList();
		rewardsClosedList = new();
		agent = agentGO.GetComponent<MoveToGoalAgent>();
		agentGO.GetComponent<PlayerCollisions>().CollisionWithRewardAction += CollectReward;
		agent.environmentController = this;
	}

	public void CollectReward(GameObject rewardGO)
	{
		Reward	reward;
		bool	endEpisode = false;

		reward = rewardsOpenList.Find(x => x.gameObject == rewardGO);
		rewardsOpenList.Remove(reward);
		reward.enabled = false;
		rewardsClosedList.Add(reward);
		endEpisode = rewardsOpenList.Count == 0;
		agent.Reinforce(reward.rewardValue, endEpisode);
	}

	public void BeginEpisode()
	{
		foreach(var reward in rewardsClosedList)
		{
			reward.enabled = true;
			rewardsClosedList.Remove(reward);
			rewardsOpenList.Add(reward);
		}
	}
}
