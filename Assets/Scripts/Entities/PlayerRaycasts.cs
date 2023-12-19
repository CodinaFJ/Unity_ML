using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycasts : MonoBehaviour
{
	[SerializeField] int	numberOfRaysUp = 7;
	[SerializeField] int	numberOfRaysDown = 3;
	[SerializeField] private LayerMask recognizableLayerMask;
	const float	RAY_LENGHT = 30;

	public RaycastHit2D[]	RecognitionRayHits;

	private List<string> gameTags = new(){
		"Untagged",
		"Player",
		"Reward",
		"Punish",
		"Ground"
	};

	private void Awake() 
	{
		RecognitionRayHits = new RaycastHit2D[numberOfRaysUp + numberOfRaysDown];	
	}

	private void Update() 
	{
		LaunchRaycasts();
	}

    void	LaunchRaycasts()
	{
		int	i = 0;
		Vector2	direction = Vector2.right;

		for (i = 0; i < numberOfRaysUp; i++)
			RecognitionRayHits[i] = CastRay(MyMath.Rotate(direction, 180 * i/(numberOfRaysUp - 1)));
		for (i = 0; i < numberOfRaysDown; i++)
			RecognitionRayHits[numberOfRaysUp + i] =
				CastRay(MyMath.Rotate(direction, (90 * i/(numberOfRaysDown - 1)) + 225));
	}

	private RaycastHit2D	CastRay(Vector2 direction)
	{
		RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, RAY_LENGHT, recognizableLayerMask);
		Debug.DrawLine(
			(Vector2) transform.position,
			(Vector2) transform.position + direction * RAY_LENGHT);
		return ray;
	}

	public float[]	GetRayCollisionDistances()
	{
		int i = 0;
		float[]	distances = new float[numberOfRaysUp + numberOfRaysDown];

		foreach(var ray in RecognitionRayHits)
		{
			distances[i] = ray.distance;
			i++;
		}
		return distances;
	}

	public int[]	GetRayCollisionIDs()
	{
		int i = 0;
		int[]	ids = new int[numberOfRaysUp + numberOfRaysDown];

		foreach(var ray in RecognitionRayHits)
		{
			if (ray.collider == null)
				continue ;
			int	id = GameTag.GetGameTagId(ray.collider.tag);
			if (id == -1)
				continue ;
			ids[i] = id;
			//this.LogDebug("Ray collision with " + ray.collider.tag + " with ID: " + ids[i]);
			i++;
		}
		return ids;
	}
}
