using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace Player
{
	[RequireComponent(typeof(ThirdPersonController),typeof(Animator))]
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
		private Animator animator;
		private int animLayer = 0;
		
		//Anim Hash
		private int animIDMeleeAttack;
		private int animIDGrounded;
		private int stateIDSpecialAttack1;
		private int stateIDSpecialAttack2;
		
		
		//Dash Variables
		private float dashCoolDownTimer = 0f;
		private bool canTriggerDash = true;
		
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED 

		private void Awake()
		{
			tpsController = GetComponent<ThirdPersonController>();
			animator = GetComponent<Animator>();
			
			animIDMeleeAttack = Animator.StringToHash("MeleeAttack");
			animIDGrounded = Animator.StringToHash("Grounded");
			stateIDSpecialAttack1 = Animator.StringToHash("SpecialAttack1");
			stateIDSpecialAttack2 = Animator.StringToHash("SpecialAttack2");

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
			HandleMoveInput(callbackContext.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext callbackContext)
		{
			if(cursorInputForLook) {
				HandleLookInput(callbackContext.ReadValue<Vector2>());
			}
		}
		public void OnJump(InputAction.CallbackContext callbackContext)
		{
			HandleJumpInput(callbackContext.ReadValueAsButton());
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
			HandleMeleeAttackInput(callbackContext.ReadValueAsButton());
		}

		public void OnSpecialAttack1(InputAction.CallbackContext callbackContext)
		{
			HandleSpecialAttack1Input(callbackContext.ReadValueAsButton());
		}
		
		public void OnSpecialAttack2(InputAction.CallbackContext callbackContext)
		{
			HandleSpecialAttack2Input(callbackContext.ReadValueAsButton());
		}

#endif

		public void HandleMoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void HandleLookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void HandleJumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void HandleMeleeAttackInput(bool isClicked) {
			if (isClicked && animator.GetBool(animIDGrounded)) {
				animator.SetTrigger(animIDMeleeAttack);
			}
			else if (!isClicked) {
				animator.ResetTrigger(animIDMeleeAttack);
			}
		}

		private void HandleSpecialAttack1Input(bool isClicked)
		{
			if (isClicked && animator.GetBool(animIDGrounded)) {
				animator.Play(stateIDSpecialAttack1,animLayer );
				//animator.SetTrigger(animIDDoSpecialAttack1);
			}
			else if (!isClicked) {
				//animator.ResetTrigger(animIDDoSpecialAttack1);
			}
		}
		
		private void HandleSpecialAttack2Input(bool isClicked)
		{
			if (isClicked && animator.GetBool(animIDGrounded)) {
				animator.Play(stateIDSpecialAttack2,animLayer );
				//animator.SetTrigger(animIDDoSpecialAttack2);
			}
			else if (!isClicked) {
				//animator.ResetTrigger(animIDDoSpecialAttack2);
			}
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