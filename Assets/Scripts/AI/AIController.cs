using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The AIController manipulates the movement and perception components of the AI.
/// And report the variables to the animator
/// </summary>
[RequireComponent(typeof(Animator),typeof(NavMeshAgent),typeof(AICharacter))]
public class AIController : MonoBehaviour
{
    private AICharacter aiCharacter;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    
    //Anim hashed ID
    private int canSeePlayerAnimID;
    private int isHatredAnimID;
    private int currentSpeedAnimID;
    private int canAttackAnimID;
    
    //Hatred Variables
    private bool isHatred = false;
    private float lossSightTimer;
    private const float wanderTimeAfterLossSight = 5f;
    [SerializeField] private float forceHatredDistance;
    
    [Header("Debug settings")]
    //Debug Settings
    [SerializeField] private bool displayForceHatredDistance;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimatorBoolCanSeePlayer();
        UpdateAnimatorBoolIsHatred();
        UpdateAnimatorWalkSpeed();
        UpdateAnimatorCanAttackPlayer();
    }

    void UpdateAnimatorBoolCanSeePlayer() 
    {
        animator.SetBool(canSeePlayerAnimID,aiCharacter.CanSeePlayer());
    }

    void UpdateAnimatorWalkSpeed()
    {
        animator.SetFloat(currentSpeedAnimID,navMeshAgent.speed);
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
        }
        else if (PlayerIsCloseEnoughToTriggerHatredState()) {
            isHatred = true;
        }
        else if (!aiCharacter.CanSeePlayer() && lossSightTimer>=wanderTimeAfterLossSight) {
            isHatred = false;
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
        if (displayForceHatredDistance) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,forceHatredDistance);
        }
    }
#endif
    
}
