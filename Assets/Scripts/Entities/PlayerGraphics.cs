using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphics : EntityGraphics
{
	PlayerMovement		playerMovement;
	PlayerCollisions	playerCollisions;

	private const float ZERO_VALUE = 0.5f;

    void Start()
    {
		playerMovement = GetComponent<PlayerMovement>();  
		playerCollisions = GetComponent<PlayerCollisions>();  
    }

    private void FixedUpdate() 
	{
		SetMovingAnimation(playerMovement.MovementDirection);
	}

	public override void SetMovingAnimation(Vector2 movingDirection)
	{
		if(Mathf.Abs(movingDirection.x) > ZERO_VALUE)
		{
			bool orientation = movingDirection.normalized.x < 0;
			spriteRenderer.flipX = orientation;
		}
		SetAnimatorFloat(VERTICAL_DIRECTION, movingDirection.y);
		SetAnimatorBool(IS_AIR, !playerCollisions.OnGround);
		SetAnimatorBool(IS_MOVING,Mathf.Abs(movingDirection.x) > ZERO_VALUE);
	}
}
