using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// The AIController manipulates the movement and perception components of the AI.
/// And report the variables to the animator
/// </summary>
[RequireComponent(typeof(Animator),typeof(NavMeshAgent),typeof(AICharacter))]
public class AIController : MonoBehaviour
{
    //Hatred Variables
    [SerializeField] [Range(0,10f)] private float wanderTimeAfterSightIsLostOffset;
    [SerializeField] private float forceHatredDistance;
    public bool isHatred { get; private set; } = false;
    private float lossSightTimer;
    private const float wanderTimeAfterSightIsLost = 5f;
    private bool isHatredInPreviousFrame;
    
    [Header("Events")] 
    [Tooltip("Called when transiting from non hatred to hatred")]
    [SerializeField] private UnityEvent OnHatred;
    [SerializeField] private UnityEvent OnNonHatred;
    
    [Header("Debug settings")]
    [SerializeField] private bool displayForceHatredDistance;
    [SerializeField] private Color displayForceHatredDistanceColor; 
    
    private AICharacter aiCharacter;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    
    //Anim hashed ID
    private int canSeePlayerAnimID;
    private int isHatredAnimID;
    private int currentSpeedAnimID;
    private int canAttackAnimID;
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        canSeePlayerAnimID = Animator.StringToHash("canSeePlayer");
        isHatredAnimID = Animator.StringToHash("isHatred");
        currentSpeedAnimID = Animator.StringToHash("currentSpeed");
        canAttackAnimID = Animator.StringToHash("canAttack");
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        aiCharacter = GetComponent<AICharacter>();

        //Potential BUG: what if instantiate an enemy and then change the animator variable? Does it guaranteed to get the latest result? 
        isHatredInPreviousFrame = animator.GetBool(isHatredAnimID);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimatorBoolCanSeePlayer();
        UpdateAnimatorBoolIsHatred();
        UpdateAnimatorWalkSpeed();
        UpdateAnimatorCanAttackPlayer();
    }

    public void SetControllerAIToHatred(bool isHatred) => this.isHatred = isHatred;

    void UpdateAnimatorBoolCanSeePlayer() => animator.SetBool(canSeePlayerAnimID,aiCharacter.CanSeePlayer());
    
    void UpdateAnimatorWalkSpeed()
    {
        Vector2 horizontalVel = navMeshAgent.velocity;
        animator.SetFloat(currentSpeedAnimID,horizontalVel.magnitude);
    }

    void UpdateAnimatorCanAttackPlayer()
    {
        animator.SetBool(canAttackAnimID,aiCharacter.PlayerIsInsideAttackArea());
    }
    
    void UpdateAnimatorBoolIsHatred()
    {
        UpdateIsHatredInternally();
        animator.SetBool(isHatredAnimID,isHatred);
    }

    void UpdateIsHatredInternally()
    {
        UpdateLossSightTimer();
        if (aiCharacter.CanSeePlayer()) {
            isHatred = true;

            if (isHatredInPreviousFrame == false) { //transited from non hatred to hatred
                isHatredInPreviousFrame = true;
                BGMManager.Instance.PlayBattleBGMInThisScene(aiCharacter);
            }
        }
        else if (PlayerIsCloseEnoughToTriggerHatredState()) {
            isHatred = true;

            if (isHatredInPreviousFrame == false) { //transited from non hatred to hatred
                isHatredInPreviousFrame = true;
                BGMManager.Instance.PlayBattleBGMInThisScene(aiCharacter);
            }
        }
        else if (!aiCharacter.CanSeePlayer() && lossSightTimer>=wanderTimeAfterSightIsLost) {
            isHatred = false;

            if (isHatredInPreviousFrame == true) { //transited from hatred to non hatred
                isHatredInPreviousFrame = false;
                BGMManager.Instance.RegisterToPlayNormalBGMInThisScene(aiCharacter);
            }
        }

    }

    void UpdateLossSightTimer()
    {
        if (aiCharacter.CanSeePlayer()) {
            lossSightTimer = 0f;
        }
        else {
            lossSightTimer += Time.deltaTime;
        }
    }
    
    bool PlayerIsCloseEnoughToTriggerHatredState()
    {
        float distance = (PlayerCharacter.Instance.GetPlayerWorldPosition() - transform.position).magnitude;
        return distance < forceHatredDistance;
    }
    
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (displayForceHatredDistance)
        {
            Gizmos.color = displayForceHatredDistanceColor;
            Gizmos.DrawWireSphere(transform.position,forceHatredDistance);
        }
    }
#endif
    
}
