using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
	{
		[Header("Smooth Movement Settings")]
        [SerializeField] [Range(0, 200)]    protected float speed = 50;
        [SerializeField] [Range(0f, 0.5f)]  protected float smoothTime = .1f;
		[SerializeField] [Range(0, 200)] 	protected float	jumpSpeed = 50;

		protected Vector2		movement = Vector2.zero;
        protected Vector2       movementVelocity;
        protected Vector2       velocity = Vector2.zero;
        protected Rigidbody2D   rigidBody;

        public float	Speed       { get => speed; set => speed = value; }
		public Vector2	Velocity    { get => velocity; set => velocity = value; }
		// Movement is the vector that determines the movement of the rigidbody and the one that is modified by the agent
		public Vector2	Movement	{ get => movement; set => movement = value; }
		public Vector2	MovementDirection	{ get => rigidBody.velocity; }

		private PlayerCollisions playerCollisions;

        protected void Start() 
        {
            rigidBody = GetComponent<Rigidbody2D>();
			playerCollisions = GetComponent<PlayerCollisions>();
        }

        protected void FixedUpdate()
        {
            MoveSmoothed(movement);
        }

		public void MoveSmoothed(Vector2 movement)
		{
			Vector2 smoothedVelocity = Vector2.SmoothDamp
			(
				movementVelocity,
				
				movement,
				ref velocity,
				smoothTime
			);
			Velocity = velocity;
			rigidBody.velocity =new Vector2((smoothedVelocity * speed).x, rigidBody.velocity.y);
		}

	public void Jump()
	{
		if (!playerCollisions.OnGround)
			return ;
		rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
	}
	}
