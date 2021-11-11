using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Reset State. Tell the AI to go back to the initial position with assigned speed
/// Read animator variables:
/// Write animator variables: hasReachedInitPos, hasBeenReset
/// </summary>
public class ResetState : StateMachineBehaviour
{

    [SerializeField] private bool useOriginalAgentSpeedForReset; 
    [SerializeField] private float speedOfAgentInReset; 
    [SerializeField] private float tolerance=0.5f;

    //Cached components
    private NavMeshAgent navMeshAgent;
    private AICharacter aiCharacter;
    private AIController aiController;

    //Cached data
    private float initSpeedOfNavMeshAgent;
    
    [ReadOnly] private float distError;
        
    //AnimID
    private int hasReachedInitPosAnimID;
    private int hasBeenResetAnimID;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        CacheReferences(animator);
        RememberInitNavMeshAgentSpeed();
        SetNavMeshAgentSpeedInResetState();
        MoveToInitPosition();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        UpdateAnimatorHasReachedInitPos(animator);
        
        if (AIPredicateHelper.HasReachedPosition(navMeshAgent.transform.position,aiCharacter.initPosition,tolerance)) {
            if (aiController.isHatred) {
                SetAnimatorHasBeenResetAnimID(animator, false);
            }
            else {
                SetAnimatorHasBeenResetAnimID(animator, true);
            }
        }
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        animator.SetBool(hasReachedInitPosAnimID, false);
        SetNavMeshAgentSpeed(initSpeedOfNavMeshAgent);
        StopNavmeshAgent();
        
        if (aiController.isHatred) {
            SetAnimatorHasBeenResetAnimID(animator, false);
        }
        else {
            SetAnimatorHasBeenResetAnimID(animator, true);
        }
        
    }
    
    private void CacheReferences(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        aiCharacter = animator.gameObject.GetComponent<AICharacter>();
        aiController = animator.gameObject.GetComponent<AIController>();
        
        hasReachedInitPosAnimID = Animator.StringToHash("hasReachedInitPos");
        hasBeenResetAnimID = Animator.StringToHash("hasBeenReset");
    }
    void RememberInitNavMeshAgentSpeed()
    {
        initSpeedOfNavMeshAgent = navMeshAgent.speed;
    }
    
    /// <summary>
    /// Set the speed of NavMeshAgent in ResetState. The speed must be greater than or equal to the original speed
    /// </summary>
    void SetNavMeshAgentSpeedInResetState()
    {
        if (useOriginalAgentSpeedForReset) 
            return ;

        speedOfAgentInReset = Mathf.Max(speedOfAgentInReset, initSpeedOfNavMeshAgent);
        SetNavMeshAgentSpeed(speedOfAgentInReset);
    }

    void MoveToInitPosition()
    {
        navMeshAgent.SetDestination(aiCharacter.initPosition);
    }
    
    void UpdateAnimatorHasReachedInitPos(Animator animator)
    {
        bool hasReachedInitPos = AIPredicateHelper.HasReachedPosition(navMeshAgent.transform.position,aiCharacter.initPosition,tolerance);
        animator.SetBool(hasReachedInitPosAnimID, hasReachedInitPos);
    }
    
    void StopNavmeshAgent()
    {
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
    }

    void SetNavMeshAgentSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    void SetAnimatorHasBeenResetAnimID(Animator animator, bool hasBeenReset)
    {
        animator.SetBool(hasBeenResetAnimID,hasBeenReset);
    }

}
