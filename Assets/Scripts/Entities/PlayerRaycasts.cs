using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycasts : MonoBehaviour
{
	[SerializeField] int	numberOfRaysUp = 7;
	[SerializeField] int	numberOfRaysDown = 3;
	[SerializeField] int	numberOfRaysSide = 3;
	[SerializeField] private LayerMask recognizableLayerMask;
	const float	RAY_LENGHT = 30;

	public RaycastHit2D[]	RecognitionRayHits;

	private void Awake() 
	{
		RecognitionRayHits = new RaycastHit2D[numberOfRaysUp + numberOfRaysDown + numberOfRaysSide * 2];	
	}

	private void Update() 
	{
		LaunchAngularRaycasts();
		LaunchParallelRaycasts();
	}

    void	LaunchAngularRaycasts()
	{
		int	i = 0;
		Vector2	direction = Vector2.right;

		for (i = 0; i < numberOfRaysUp; i++)
			RecognitionRayHits[i] = CastRayAngular(MyMath.Rotate(direction, 180 * i/(numberOfRaysUp - 1)));
		for (i = 0; i < numberOfRaysDown; i++)
			RecognitionRayHits[numberOfRaysUp + i] =
				CastRayAngular(MyMath.Rotate(direction, (180 * i/(numberOfRaysDown - 1)) + 180));
	}

	void	LaunchParallelRaycasts()
	{
		int j = 0;

		for (int i = 0 ; i < numberOfRaysSide * 2; i += 2)
		{
			float pos_y =  transform.position.y - 1.7f + 4 * (1 - ((float) j) / numberOfRaysSide);
			RecognitionRayHits[i + numberOfRaysUp + numberOfRaysDown] =
				CastRayParallel(new Vector2(transform.position.x, pos_y), Vector2.right);

			RecognitionRayHits[i + numberOfRaysUp + numberOfRaysDown + 1] =
				CastRayParallel(new Vector2(transform.position.x, pos_y), Vector2.left);
			j++;
		}
	}

	private RaycastHit2D	CastRayAngular(Vector2 direction)
	{
		RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, RAY_LENGHT, recognizableLayerMask);
		Debug.DrawLine(
			(Vector2) transform.position,
			(Vector2) transform.position + direction * RAY_LENGHT);
		return ray;
	}

	private RaycastHit2D	CastRayParallel(Vector2 start, Vector2 direction)
	{
		RaycastHit2D ray = Physics2D.Raycast(start, direction, RAY_LENGHT, recognizableLayerMask);
		Debug.DrawLine(
			(Vector2) start,
			(Vector2) start + direction * RAY_LENGHT);
		return ray;
	}

	public float[]	GetRayCollisionDistances()
	{
		int i = 0;
		float[]	distances = new float[numberOfRaysUp + numberOfRaysDown + numberOfRaysSide * 2];

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
		int[]	ids = new int[numberOfRaysUp + numberOfRaysDown + numberOfRaysSide * 2];

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
