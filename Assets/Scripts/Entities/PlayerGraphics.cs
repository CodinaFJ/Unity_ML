using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphics : EntityGraphics
    {
        InputService    inputService;
        PlayerMovement  playerMovement;

        private void Start() {
            inputService = ServiceLocator.Instance.GetService<InputService>();  
            playerMovement = GetComponent<PlayerMovement>();
            inputService.MoveAction += SetMovingAnimation;
        }

        private void OnEnable() {
            inputService = ServiceLocator.Instance.GetService<InputService>();   
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void FixedUpdate() 
		{
			UpdatePlayerAnimation(playerMovement.Velocity);
            SetMovingAnimation(playerMovement.Velocity);
        }

        private void UpdatePlayerAnimation(Vector2 movement)
        {
            if(movement != Vector2.zero)
            {
                SetAnimatorFloat(HORIZONTAL_DIRECTION, movement.normalized.x);
                SetAnimatorFloat(VERTICAL_DIRECTION, movement.normalized.y);
            }
        }

        public override void SetMovingAnimation(Vector2 movingDirection)
        {
            SetAnimatorBool(IS_MOVING, movingDirection != Vector2.zero);
        }
    }
