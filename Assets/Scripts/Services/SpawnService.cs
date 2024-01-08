using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnService
{
	private readonly Dictionary<GameObject, SpawnZone> spawnZones = new();
	public SpawnService()
	{
		this.LogDebug("Started");
	}

	public void RegisterSpawnerZone(GameObject spawnObject, SpawnZone spawnZone)
	{
		this.LogDebug("Register spawn zone with object name" + spawnObject.name);
		spawnZones.Add(spawnObject, spawnZone);
	}

	public bool AssertObjectSpawnedInZone(Vector2 agentPosition, GameObject spawnObject)
	{
		if (spawnZones[spawnObject].AssertContainsPosition(agentPosition))
			return true;
		return false;
	}

	public Vector2 GetPositionInSpawnZone(GameObject spawnObject)
	{
		if (spawnZones.ContainsKey(spawnObject))
		{
			return spawnZones[spawnObject].GetRandomPositionInZone();
		}
		return Vector2.zero;
	}
}
