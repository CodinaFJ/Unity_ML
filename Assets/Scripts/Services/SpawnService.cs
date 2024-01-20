using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnService
{
	struct SpawnZoneObject
	{
		public GameObject	ObjectToSpawn;
		public SpawnZone	SpawnZone;
	}

	private readonly List<SpawnZoneObject> spawnZones;
	public SpawnService()
	{
		spawnZones = new();
		spawnZones.Clear();
		this.LogDebug("Started");
	}

	public void RegisterSpawnerZone(GameObject spawnObject, SpawnZone spawnZone)
	{
		spawnZones.Add(new SpawnZoneObject{ ObjectToSpawn = spawnObject, SpawnZone = spawnZone});
	}

	public bool AssertObjectSpawnedInZone(Vector2 agentPosition, GameObject spawnObject)
	{
		if (spawnZones.Find(x => x.ObjectToSpawn == spawnObject).SpawnZone.AssertContainsPosition(agentPosition))
			return true;
		return false;
	}

	public Vector2 GetPositionInSpawnZone(GameObject spawnObject)
	{
		if (spawnZones.Exists(x => x.ObjectToSpawn == spawnObject))
		{
			return spawnZones.Find(x => x.ObjectToSpawn == spawnObject).SpawnZone.GetRandomPositionInZone();
		}
		return Vector2.zero;
	}
}
