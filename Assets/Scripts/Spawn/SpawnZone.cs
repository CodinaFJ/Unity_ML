using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
	[SerializeField] private GameObject spawnObject;

	private SpawnService		spawnService;
	private Collider2D			colliderBox;

    public GameObject			SpawnObject {get => spawnObject;}

	private void Start() 
	{
		spawnService = ServiceLocator.Instance.GetService<SpawnService>();
		spawnService.RegisterSpawnerZone(spawnObject, this);
		colliderBox = GetComponent<Collider2D>();
	}

	public bool AssertContainsPosition(Vector2 pos)
	{
		if (colliderBox.bounds.Contains(pos))
			return true;
		return false;
	}

	public Vector2 GetRandomPositionInZone()
	{
		Bounds bounds = colliderBox.bounds;

		return new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
	}
}
