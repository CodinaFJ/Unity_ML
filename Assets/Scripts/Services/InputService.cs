using System;
using UnityEngine;

public class InputService
	{
		public Action<Vector2>	MoveAction		{ get; set;}

		public Vector2			Movement 		{ get; set;} = new();

		private readonly GameInputActions inputActions;

		public InputService()
		{
			inputActions = new GameInputActions();

			EnableInput();
			LinkInput();
			SetInputActive(true);
			this.LogDebug("Started");
		}

		~InputService()
		{
			DisableInput();
			UnlinkInputs();
		}

		public void SetInputActive(bool setActive)
		{
			SetPlayerInputActive(setActive);
			SetUIInputActive(setActive);
		}

		public void SetPlayerInputActive(bool setActive)
		{
			if (setActive)
				inputActions.Player.Enable();
			else
				inputActions.Player.Disable();
		}

		public void SetUIInputActive(bool setActive)
		{
			if (setActive)
				inputActions.UI.Enable();
			else
				inputActions.UI.Disable();
		}

		private void UpdateMovementVector(Vector2 movement)
		{
			this.Movement = movement;
		}

		private void EnableInput()
		{
			inputActions.Player.Movement.Enable();
		}

		private void LinkInput()
		{
			inputActions.Player.Movement.performed += context => MoveAction?.Invoke(inputActions.Player.Movement.ReadValue<Vector2>());
			MoveAction += UpdateMovementVector;
		}

		private void DisableInput()
		{
			inputActions.Player.Movement.Disable();
		}

		private void UnlinkInputs()
		{
			inputActions.Player.Movement.performed -= context => MoveAction?.Invoke(inputActions.Player.Movement.ReadValue<Vector2>());
			MoveAction -= UpdateMovementVector;
		}
	}
