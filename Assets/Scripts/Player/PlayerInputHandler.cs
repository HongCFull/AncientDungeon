using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace Player
{
	[RequireComponent(typeof(ThirdPersonController),typeof(Animator),typeof(PlayerCharacter))]
	public class PlayerInputHandler : MonoBehaviour
	{
		[Header("Custom Input Settings")] 
		[SerializeField] private bool enableLookInput = true;
		
		[Header("Cool Down Settings")]
		[Tooltip("The time that allows you to trigger double tap after tapping one key")]
		[SerializeField][Range(0,10)] private float doubleTapTimeInterval;
		
		[Tooltip("The time has to pass after the double tap in order to it again")]
		[SerializeField][Range(0,10)] private float doubleTapCoolDown;
		
		// [SerializeField] [Range(0f , 10f)]private float dashCoolDownTime; 

		[Header("References")] 
		[SerializeField] private ShirleySkillManager shirleySkillManager;
		[SerializeField] private InputActionReference wKeyDoubleTapAction;
		[SerializeField] private InputActionReference aKeyDoubleTapAction;
		[SerializeField] private InputActionReference sKeyDoubleTapAction;
		[SerializeField] private InputActionReference dKeyDoubleTapAction;
		[SerializeField] private PlayerMainCamera playerCamera;

		//[Header("Default Character Input Values")]
		public Vector2 move { get; private set; }
		public Vector2 look{ get; private set; }
		public bool jump { get; set; } = false;
		public bool sprint { get; private set; } =false ;
		
		
	#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")] 
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
	#endif
		
		private ThirdPersonController tpsController;
		private Animator animator;
		private PlayerCharacter playerCharacter;
		private  int activeAnimLayer = 0;
		
		//Hashed anim ID
		private int animIDMeleeAttack;
		private int animIDGrounded;

		//Dash Variables
		// private float dashCoolDownTimer = 0f;
		// private bool canTriggerDash = true;
		  
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
		
		
		private void HashAnimID()
		{
			animIDMeleeAttack = Animator.StringToHash("meleeAttack");
			animIDGrounded = Animator.StringToHash("grounded");
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

			public void OnSkillQInput(InputAction.CallbackContext callbackContext)
			{
				HandleSkillQInput(callbackContext.ReadValueAsButton());
			}
			
			public void OnSkillEInput(InputAction.CallbackContext callbackContext)
			{
				HandleSkillEInput(callbackContext.ReadValueAsButton());
			}

			public void OnCharacterAwake(InputAction.CallbackContext callbackContext)
			{
				HandleCharacterAwakeInput(callbackContext.ReadValueAsButton());
			}

			public void OnZoomCamera(InputAction.CallbackContext callbackContext)
			{
				HandleZoomCameraInput(callbackContext.ReadValue<Vector2>());
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
				if(callbackContext.performed)
					shirleySkillManager.PerformSkill(KeyCode.LeftShift);
			}
		
			private void HandleSkillQInput(bool isClicked)
			{
				if (isClicked) 
					shirleySkillManager.PerformSkill(KeyCode.Q);
			}
		
			private void HandleSkillEInput(bool isClicked)
			{
				if (isClicked) 
					shirleySkillManager.PerformSkill(KeyCode.E);
			}
		
			private void HandleCharacterAwakeInput(bool isClicked)
			{
				if (isClicked) {
					shirleySkillManager.PerformSkill(KeyCode.LeftControl);
				}
			}

			private void HandleZoomCameraInput(Vector2 scrollVector)
			{
				if (scrollVector.y > 0)
					playerCamera.ZoomOutCameraByOneLevel();
				else if(scrollVector.y < 0)
					playerCamera.ZoomInCameraByOneLevel();
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
