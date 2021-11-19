using System;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace Player
{
	[RequireComponent(typeof(ThirdPersonController),typeof(Animator),typeof(PlayerCharacter))]
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Custom Input Settings")] 
		[SerializeField] private bool enableLookInput = true;
		
		[Tooltip("The time that allows you to trigger double tap after tapping one key")]
		[SerializeField][Range(0,10)] private float doubleTapTimeInterval;
		
		[Tooltip("The time has to pass after the double tap in order to it again")]
		[SerializeField][Range(0,10)] private float doubleTapCoolDown;
		
		[SerializeField] private InputActionReference wKeyDoubleTapAction;
		[SerializeField] private InputActionReference aKeyDoubleTapAction;
		[SerializeField] private InputActionReference sKeyDoubleTapAction;
		[SerializeField] private InputActionReference dKeyDoubleTapAction;

		[Header("Default Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump = false;
		public bool sprint =false ;
		public bool meleeAttack = false;
		
		[Header("Movement Settings")]
		public bool analogMovement;
		[SerializeField] [Range(0f , 10f)]private float dashCoolDownTime; 
		
	#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
	#endif
		
		private ThirdPersonController tpsController;
		private Animator animator;
		private PlayerCharacter playerCharacter;
		private int activeAnimLayer = 0;
		
		//Hashed anim ID
		private int animIDMeleeAttack;
		private int animIDGrounded;
		private int stateIDSpecialAttack1;
		private int stateIDSpecialAttack2;

		//Dash Variables
		private float dashCoolDownTimer = 0f;
		private bool canTriggerDash = true;
		
		//Keys that can be double tapped 
		//W key
		private bool enableWKeyDoubleTab = true;
		private float wKeylastTapTime = 0f;
		
		//A key
		private bool enableAKeyDoubleTab= true;
		private float aKeylastTapTime = 0f;
		
		//S key
		private bool enableSKeyDoubleTab= true;
		private float sKeylastTapTime = 0f;
		
		//D key
		private bool enableDKeyDoubleTab= true;
		private float dKeylastTapTime = 0f;

		
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED 

		private void Awake()
		{
			tpsController = GetComponent<ThirdPersonController>();
			animator = GetComponent<Animator>();
			playerCharacter = GetComponent<PlayerCharacter>();
			
			HashAnimID();
			AssignDoubleTapActionCallback();
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
		
		private void HashAnimID()
		{
			animIDMeleeAttack = Animator.StringToHash("MeleeAttack");
			animIDGrounded = Animator.StringToHash("Grounded");
			stateIDSpecialAttack1 = Animator.StringToHash("SpecialAttack1");
			stateIDSpecialAttack2 = Animator.StringToHash("SpecialAttack2");
		}

		public void EnableLookInput()
		{
			enableLookInput = true;
		}

		public void DisableLookInput()
		{
			enableLookInput = false;
		}
		
		#region Input Callback events

		//Invoke event method
			public void OnMove(InputAction.CallbackContext callbackContext)
			{
				HandleMoveInput(callbackContext.ReadValue<Vector2>());
			}

			public void OnLook(InputAction.CallbackContext callbackContext)
			{
				if(cursorInputForLook && enableLookInput) {
					HandleLookInput(callbackContext.ReadValue<Vector2>());
				}
			}
			
			public void OnJump(InputAction.CallbackContext callbackContext)
			{
				HandleJumpInput(callbackContext.ReadValueAsButton());
			}

			public void OnDash(InputAction.CallbackContext callbackContext)
			{
				HandleDashInput(callbackContext);
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

			public void OnCharacterAwake(InputAction.CallbackContext callbackContext)
			{
				HandleCharacterAwakeInput(callbackContext.ReadValueAsButton());
			}
		#endregion

#endif

		#region Handle callback logic

			#region DoubleTap
			
				private void AssignDoubleTapActionCallback()
				{
					wKeyDoubleTapAction.action.performed += context => DetectWKeyDoubleTap();
					aKeyDoubleTapAction.action.performed += context => DetectAKeyDoubleTap();
					sKeyDoubleTapAction.action.performed += context => DetectSKeyDoubleTap();
					dKeyDoubleTapAction.action.performed += context => DetectDKeyDoubleTap();
				}

				private void DetectWKeyDoubleTap()
				{
					if (!enableWKeyDoubleTab)
						return;
					
					if ( Time.time - wKeylastTapTime < doubleTapTimeInterval) {
						wKeylastTapTime = 0;	//reset timer
						enableWKeyDoubleTab = false;
						sprint = true;
						Invoke(nameof(EnableWKeyDoubleTap),doubleTapCoolDown);
					}

					wKeylastTapTime = Time.time;
				}
				private void EnableWKeyDoubleTap()
				{
					enableWKeyDoubleTab = true;
				}
				
				private void DetectAKeyDoubleTap()
				{
					if (!enableAKeyDoubleTab)
						return;

					if (Time.time - aKeylastTapTime < doubleTapTimeInterval) {
						aKeylastTapTime = 0;	//reset timer
						enableAKeyDoubleTab = false;
						sprint = true;
						Invoke(nameof(EnableAKeyDoubleTap),doubleTapCoolDown);
					}

					aKeylastTapTime = Time.time;
				}
				private void EnableAKeyDoubleTap()
				{
					enableAKeyDoubleTab = true;
				}
				
				private void DetectSKeyDoubleTap()
				{
					if (!enableSKeyDoubleTab)
						return;

					if (Time.time - sKeylastTapTime < doubleTapTimeInterval) {
						sKeylastTapTime = 0;	//reset timer
						enableSKeyDoubleTab = false;
						sprint = true;
						Invoke(nameof(EnableSKeyDoubleTap),doubleTapCoolDown);
					}

					sKeylastTapTime = Time.time;
				}
				private void EnableSKeyDoubleTap()
				{
					enableSKeyDoubleTab = true;
				}
				
				private void DetectDKeyDoubleTap()
				{
					if (!enableDKeyDoubleTab)
						return;

					if (Time.time - dKeylastTapTime < doubleTapTimeInterval) {
						dKeylastTapTime = 0;	//reset timer
						enableDKeyDoubleTab = false;
						sprint = true;
						Invoke(nameof(EnableDKeyDoubleTap),doubleTapCoolDown);

					}

					dKeylastTapTime = Time.time;
				}
				private void EnableDKeyDoubleTap()
				{
					enableDKeyDoubleTab = true;
				}

			#endregion
			
			public void HandleMoveInput(Vector2 newMoveDirection)
			{
				if (newMoveDirection == Vector2.zero)
					sprint = false;
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
		
			private void HandleMeleeAttackInput(bool isClicked) 
			{
				if (isClicked && animator.GetBool(animIDGrounded)) {
					sprint = false;
					animator.SetTrigger(animIDMeleeAttack);
				}
				else if (!isClicked) {
					animator.ResetTrigger(animIDMeleeAttack);
				}
			}

			private void HandleDashInput(InputAction.CallbackContext callbackContext)
			{
				if (canTriggerDash && callbackContext.performed) {
					sprint = false;
					canTriggerDash = false;
					tpsController.TriggerDash();
				}
			}
		
			private void HandleSpecialAttack1Input(bool isClicked)
			{
				if (isClicked && animator.GetBool(animIDGrounded)) {
					sprint = false;
					animator.Play(stateIDSpecialAttack1,activeAnimLayer );
					//animator.SetTrigger(animIDDoSpecialAttack1);
				}
				else if (!isClicked) {
					//animator.ResetTrigger(animIDDoSpecialAttack1);
				}
			}
		
			private void HandleSpecialAttack2Input(bool isClicked)
			{
				if (isClicked && animator.GetBool(animIDGrounded)) {
					sprint = false;
					animator.Play(stateIDSpecialAttack2,activeAnimLayer );
					//animator.SetTrigger(animIDDoSpecialAttack2);
				}
				else if (!isClicked) {
					//animator.ResetTrigger(animIDDoSpecialAttack2);
				}
			}
		
			private void HandleCharacterAwakeInput(bool isClicked)
			{
				if (isClicked && animator.GetBool(animIDGrounded) && playerCharacter.playerAttackMode == PlayerAttackMode.NORMAL) {
					playerCharacter.SetCharacterToAwakeMode();
				}
			}
		#endregion
		
		
		
		
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
