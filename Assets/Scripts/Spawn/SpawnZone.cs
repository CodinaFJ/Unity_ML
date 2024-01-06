using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
	public Tag gameTag;

    [SerializeField] private List<GameObject>	objectInSpawnZone = new();
	private SpawnService		spawnService;
	private Collider2D			colliderBox;

	private void Start() {
		spawnService = ServiceLocator.Instance.GetService<SpawnService>();
		spawnService.RegisterSpawnerZone(this);
		colliderBox = GetComponent<Collider2D>();
	}

	public bool AssertObjectSpawnedInZone(Vector2 pos)
	{
		if (colliderBox.bounds.Contains(pos))
		{
			this.LogDebug("In bounds");
			return true;
		}
		return false;
	}
}
