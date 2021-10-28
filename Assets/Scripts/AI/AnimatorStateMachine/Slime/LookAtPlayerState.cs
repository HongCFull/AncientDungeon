using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Look at the player instantly or through interpolation.
/// Note that this manipulates the root motion option of animator internally or set rotation won't work
/// </summary>
public class LookAtPlayerState : StateMachineBehaviour
{
    [SerializeField] private bool turnInstantly;
    
    [Header("Instant turning settings")]
    [Tooltip("Only usable when turnInstantly is true. The delay time percentage of this state to focus on player")]
    [SerializeField][Range(0,1)] private float delayTimePercentage;
    
    [Header("Smooth turning settings")]
    [SerializeField] private float turningSpeed;
    
    private NavMeshAgent navMeshAgent;
    private bool originalRMOption;
    private float originalAgentSpeed;
    private bool finishedLookAtPlayerOnce = false;

    //Smoothing variable
    private float delayCounter = 0f;
    private float calculatedDelayTime ;
   
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        CacheReferences(animator);
        CalculateDelayTime(stateInfo);
        DisableRootMotion(animator);
        SetNavMeshAgentSpeedTo(0);
       // SetNavMeshAgentDestination(navMeshAgent.gameObject.transform.position);
    }
    

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (turnInstantly)
            LookAtPlayerInstantlyWithDelay();
        else
            LookAtPlayerWithSmoothing();
    }

  
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.applyRootMotion = originalRMOption;
        SetNavMeshAgentSpeedTo(originalAgentSpeed);
    }

    private void CacheReferences(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        originalRMOption = animator.applyRootMotion;
        originalAgentSpeed = navMeshAgent.speed;

    }

    void CalculateDelayTime(AnimatorStateInfo stateInfo)
    {
        calculatedDelayTime  = delayTimePercentage * stateInfo.length ;
    }
    
    /// <summary>
    /// Disable RM of the animator. It is required if manually set the rotation of the gameObject which has an animator
    /// </summary>
    /// <param name="animator"></param>
    void DisableRootMotion(Animator animator)
    {
        animator.applyRootMotion = false;
    }

    void LookAtPlayerInstantlyWithDelay()
    {
        delayCounter += Time.deltaTime;
        if (!finishedLookAtPlayerOnce && delayCounter >= calculatedDelayTime) {
            finishedLookAtPlayerOnce = true;
            LookAtPlayerInstantly();
        }
    }

    void LookAtPlayerInstantly()
    {
        navMeshAgent.transform.LookAt(PlayerCharacter.Instance.transform);
    }

    void LookAtPlayerWithSmoothing()
    {
        Quaternion currentRotation = navMeshAgent.gameObject.transform.rotation;
        Vector3 rotationalVec =  PlayerCharacter.Instance.transform.position - navMeshAgent.gameObject.transform.position ;
        Quaternion targetRotation = Quaternion.LookRotation(rotationalVec);
        navMeshAgent.transform.rotation = Quaternion.Lerp(currentRotation,targetRotation,turningSpeed * Time.deltaTime);
    }

    void SetNavMeshAgentSpeedTo(float speed)
    {
        navMeshAgent.speed = speed;
    }
    
    
}
