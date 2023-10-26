using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
	{
		[Header("Smooth Movement Settings")]
        [SerializeField] [Range(0, 200)]    protected float speed = 50;
        [SerializeField] [Range(0f, 0.5f)]  protected float smoothTime = .3f;

		private PlayerGraphics	playerGraphics;

        protected Vector2       movementVelocity;
        protected Vector2       velocity = Vector2.zero;
        protected Rigidbody2D   rigidBody;

        public float	Speed       { get => speed; set => speed = value; }
		public Vector2	Velocity    { get => velocity; set => velocity = value; }

        protected void Start() 
        {
            rigidBody = GetComponent<Rigidbody2D>();
			playerGraphics = GetComponent<PlayerGraphics>();  
        }

        protected void FixedUpdate()
        {
            MoveSmoothed(ServiceLocator.Instance.GetService<InputService>().Movement);
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
            rigidBody.velocity = smoothedVelocity * speed;
        }
	}
