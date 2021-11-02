using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using Unity.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace TPSTemplate
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonController : MonoBehaviour
	{
		//Player
		[Header("Player")] 
		
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 2.0f;
		
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 5.335f;
		
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;
		
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Tooltip("For tuning rotation time. The speed threshold to identify if the player is considered as moving ")]
		[Range(0.1f, 2f)]
		[SerializeField] private float moveSpeedThreshold;

		[Tooltip("This rotation smooth time is applied to the player when the player is identified as idling")]
		[ReadOnly]  public float idleRotationSmoothTime = 0.015f; 
		
		[Tooltip("This rotation smooth time is applied to the player when the player is identified as moving")]
		[ReadOnly]  public float moveRotationSmoothTime = 0.07f; 

		
		[Space(10)]
		public float JumpHeight = 1.2f;
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;
		
		
		[Header("Player Grounded Settings")]
		
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;
		
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;
		
		[Tooltip("Slope limit for sliding")] 
		[SerializeField] private float slopeLimit;	
		
		//Cinemachine
		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;
		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		//
		
		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;
		
		// player
		private bool grounded = true;
		private bool isSliding = false;
		private bool enableWaling = true;
		private bool enableGravity = true;
		
		private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
		private Vector3 groundHitNormal;
		
		
		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;
		private int _animIDMeleeAttack;
		private int _animIDIsInComboState;
		private int _animIDDash;
		private int _animIDTurningAngle;

		private Animator _animator;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null) {
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{

			InitializeVariables();
			AssignAnimationIDs();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}
		

		private void Update()
		{
			HandleInputInComboState();
			UpdateAnimatorTurningAngle(GetNormalizedInputVectorInCameraSpace());

			if (enableWaling) {
				Move();
			}
			else {
				ResetAnimatorMovementPara();
			}
			
			JumpAndGravity();
			GroundedCheck();
			CheckSliding();
			HandleMeleeAttackInput();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		void OnControllerColliderHit (ControllerColliderHit hit) {
			groundHitNormal = hit.normal;
		}
		
///========================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================
/// public functions
///========================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================

		/// <summary>
		/// In the animation transition, the enable character movement is enabled before transiting to any other state.
		/// </summary>
		public void EnableCharacterWalking() 
		{
			enableWaling = true;
		}  	

		public void DisableCharacterWalking() 
		{
			enableWaling = false;
		}

		public void EnableGravity()
		{
			enableGravity = true;
		}
		
		public void DisableGravity()
		{
			enableGravity = false;
		}
		
		public void TriggerDash()
		{
			_animator.SetTrigger(_animIDDash);
		}
		
///====================================================================================================================================================================================================================================================================================
/// private functions
///====================================================================================================================================================================================================================================================================================

		private void InitializeVariables()
		{
			_animator = GetComponent<Animator>();
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
			_speed = MoveSpeed;
		}
		
		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
			_animIDMeleeAttack = Animator.StringToHash("MeleeAttack");
			_animIDDash = Animator.StringToHash("Dash");
			_animIDIsInComboState = Animator.StringToHash("IsInComboState");
			_animIDTurningAngle = Animator.StringToHash("TurningAngle");
			
		}

		private void CameraRotation()
		{
			// if there is an input and camera position is not fixed
			if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition) {
				_cinemachineTargetYaw += _input.look.x * Time.deltaTime;
				_cinemachineTargetPitch += _input.look.y * Time.deltaTime;
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		}
		

		//TODO: when the player is not moving e.g. speed ~= 0 -> make the rotation smooth time to 0.01. Else make it 0.07
		private void Move()
		{

			float targetSpeed;
			if (_input.move == Vector2.zero) 
				targetSpeed = 0.0f;
			else {
				//targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
				targetSpeed = SprintSpeed;
			}

			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
			//Debug.Log("speed = " + currentHorizontalSpeed);
			//ApplyRotationSmoothTimeOnGround(currentHorizontalSpeed);
			
			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

			// normalise input direction
			Vector3 inputDirection = GetNormalizedInputVector();

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero) {
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

				// rotate to face input direction relative to camera position
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}

			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

			Vector3 movementVector;
			if (enableGravity) {
				movementVector = targetDirection.normalized * (_speed * Time.deltaTime) +
				                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
				
				if (isSliding) {
					float slideFriction = 0.01f;
					float slideSpeed = 10f;
					movementVector =slideSpeed * new Vector3(Time.deltaTime*(1f - groundHitNormal.y) * groundHitNormal.x * (1f - slideFriction)
												, 0
												, Time.deltaTime*(1f - groundHitNormal.y) * groundHitNormal.z * (1f - slideFriction))+
					                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
				}
			}
			
			else
				movementVector = targetDirection.normalized * (_speed * Time.deltaTime);
			
			_controller.Move(movementVector);
			
			_animator.SetFloat(_animIDSpeed, _animationBlend);
			_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);

		}

		private void JumpAndGravity()
		{
			if (grounded) {
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				_animator.SetBool(_animIDJump,false);
				_animator.SetBool(_animIDFreeFall, false);

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f) {
					_verticalVelocity = -2f;
				}

				// Jump

				if (_input.jump && _jumpTimeoutDelta <= 0.0f ) {
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					// update animator if using character
					_animator.SetBool(_animIDJump,true);
					//Debug.Log("physically jumped");
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f) {
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else {
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f) {
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else {
					_animator.SetBool(_animIDFreeFall, true);
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			ApplyGravity();

		}
		

		private void ApplyGravity()
		{
			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity) {
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
			_animator.SetBool(_animIDGrounded, grounded);
			
		}
		private void CheckSliding()
		{
			if (!grounded) {
				isSliding = false;
				return;
			}

			float angle = Vector3.Angle(Vector3.up, groundHitNormal);
			isSliding = angle >= slopeLimit;
			//Debug.Log("Angle = "+angle+" isSliding = "+isSliding);
			
		}
		
		private void ResetAnimatorMovementPara() {
			_animator.SetFloat(_animIDSpeed, 0);
			_animator.SetFloat(_animIDMotionSpeed, 0);
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}



		private void HandleMeleeAttackInput() 
		{
			if (_input.meleeAttack) {
				_input.meleeAttack = false;
				_animator.SetTrigger(_animIDMeleeAttack);
			}
			else {
				_animator.ResetTrigger(_animIDMeleeAttack);
			}
		}
		
		private void HandleInputInComboState()
		{
			if(!_animator.GetBool(_animIDIsInComboState))
				return;
		}

		private Vector3 GetNormalizedInputVector()
		{
			return new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
		}
		
		private Vector3 GetNormalizedInputVectorInCameraSpace()
		{
			Vector3 inputVector = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
			Vector3 cameraForwardVector = _mainCamera.transform.forward;
			cameraForwardVector.y = 0;
			
			Quaternion shiftVector = Quaternion.FromToRotation(Vector3.forward, cameraForwardVector.normalized);
			Vector3 result = (shiftVector * inputVector);
			result.y = 0;
			return result.normalized;
		}

		private void UpdateAnimatorTurningAngle(Vector3 inputVectorInCameraSpace)
		{
			if (inputVectorInCameraSpace == Vector3.zero)
			{
				_animator.SetFloat(_animIDTurningAngle,0f);
				return;
			}
			bool isTurningRight = Vector3.Cross(transform.forward.normalized,inputVectorInCameraSpace).y< 0;
			float angle = Mathf.Acos(Vector3.Dot(transform.forward.normalized, inputVectorInCameraSpace))/Mathf.PI*180f;
			angle = isTurningRight ? -angle : angle;
			_animator.SetFloat(_animIDTurningAngle,angle);
		}
		
		
#if  UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
#endif
	}
}

//OBSOLETE FUNCTIONS:
/*
private void ApplyRotationSmoothTimeOnGround(float currentHorizontalSpeed ) 
{
	//if the player is jumping, dont adjust the rotation smooth time
	if (!grounded) return;
	if (currentHorizontalSpeed>=Mathf.Epsilon && currentHorizontalSpeed<= moveSpeedThreshold) {
		RotationSmoothTime = 0.015f;
	}
	else {
		RotationSmoothTime = 0.07f;
	}
}
*/