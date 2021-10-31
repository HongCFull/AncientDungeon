using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace TPSTemplate
{
	[RequireComponent(typeof(ThirdPersonController))]
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Default Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump = false;
		public bool sprint =false ;
		public bool meleeAttack = false;
		
		[Header("Movement Settings")]
		public bool analogMovement;
		[SerializeField] [Range(0f,10f)]private float dashCoolDownTime; 
		
	#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
	#endif
		
		private ThirdPersonController tpsController;
		
		//Dash Variables
		private float dashCoolDownTimer = 0f;
		private bool canTriggerDash = true;
		
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED 

		private void Awake()
		{
			tpsController = GetComponent<ThirdPersonController>();
		}

		private void Update()
		{
			if (!canTriggerDash) {
				dashCoolDownTimer += Time.deltaTime;
				if (dashCoolDownTimer >= dashCoolDownTime) {
					dashCoolDownTimer = 0f;
					canTriggerDash = true;
				}
			}
		}

		//Invoke event method
		public void OnMove(InputAction.CallbackContext callbackContext)
		{
			MoveInput(callbackContext.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext callbackContext)
		{
			if(cursorInputForLook) {
				LookInput(callbackContext.ReadValue<Vector2>());
			}
		}
		public void OnJump(InputAction.CallbackContext callbackContext)
		{
			JumpInput(callbackContext.ReadValueAsButton());
		}

		public void OnDash(InputAction.CallbackContext callbackContext)
		{
			if (canTriggerDash && callbackContext.performed) {
				canTriggerDash = false;
				tpsController.TriggerDash();
			}
		}
		
		public void OnMeleeAttack(InputAction.CallbackContext callbackContext)
		{
			MeleeAttackInput(callbackContext.ReadValueAsButton());
		}

#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void MeleeAttackInput(bool newAttackState) {
			meleeAttack = newAttackState;
		}
		
#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
		
#endif

	}
	
}

/* Obsolete
// Send MSG method
		
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
			
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		
		public void OnMeleeAttack(InputValue value) {
			//Debug.Log("OnMeleeAttack is called!");
			MeleeAttackInput(value.isPressed);
		}
		

*/