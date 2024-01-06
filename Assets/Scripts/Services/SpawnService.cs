using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnService
{
	private readonly List<SpawnZone> spawnZones = new();
	public SpawnService()
	{
		this.LogDebug("Started");
	}

	public void RegisterSpawnerZone(SpawnZone spawnZone)
	{
		this.LogDebug("Register spawn zone with tag" + spawnZone.tag.ToString());
		spawnZones.Add(spawnZone);
	}

	public bool AssertObjectSpawnedInZone(Vector2 agentPosition, string tag)
	{
		foreach(var spawnZone in spawnZones.FindAll(x => x.gameTag.ToString() == tag))
		{
			if (spawnZone.AssertObjectSpawnedInZone(agentPosition))
				return true;
		}
		return false;
	}
}
