using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnService
{
	private readonly Dictionary<GameObject, SpawnZone> spawnZones;
	public SpawnService()
	{
		spawnZones = new();
		spawnZones.Clear();
		this.LogDebug("Started");
	}

	public void RegisterSpawnerZone(GameObject spawnObject, SpawnZone spawnZone)
	{
		if (spawnZones.ContainsKey(spawnObject))
		{
			spawnZones.Remove(spawnObject);
		}
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
