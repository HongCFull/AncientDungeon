using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandWanderState : StateMachineBehaviour
{
    [SerializeField] private float tolerance=0.5f;
    [SerializeField] private float randWanderRadius;
    [SerializeField] private bool enableStrafing;
    private NavMeshAgent navMeshAgent;
    private AICharacter aiCharacter;
    
    private int hasReachedRandWanderingPosAnimID;
    private Vector3 destination;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheReferences(animator);

        if (enableStrafing) {
            SetNavmeshAgentStrafingTo(true);
        }
        
        GoToRandPosition();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        UpdateAnimatorHasReachedRandWanderPosition(animator);
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        animator.SetBool(hasReachedRandWanderingPosAnimID, false);
        SetNavmeshAgentStrafingTo(false);
        StopNavmeshAgent();
    }

    private void CacheReferences(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();

        aiCharacter = animator.gameObject.GetComponent<AICharacter>();

        hasReachedRandWanderingPosAnimID = Animator.StringToHash("hasReachedRandWanderingPos");
    }

    void GoToRandPosition()
    {
        Vector3 randomDistanceVec = Random.insideUnitSphere * randWanderRadius;

        Vector3 targetPos = aiCharacter.GetAICharacterWorldPosition() + randomDistanceVec;
        NavMeshHit hit;
        NavMesh.SamplePosition(targetPos, out hit,randWanderRadius,1);
        destination = hit.position;
        navMeshAgent.SetDestination(destination);    
    }
    
    void UpdateAnimatorHasReachedRandWanderPosition(Animator animator)
    {
        bool hasReachedFleePosition = AIPredicateHelper.HasReachedPosition(navMeshAgent.transform.position,destination,tolerance);
        animator.SetBool(hasReachedRandWanderingPosAnimID, hasReachedFleePosition);
    }
    
    void StopNavmeshAgent()
    {
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
    }

    void SetNavmeshAgentStrafingTo(bool strafe)
    {
        navMeshAgent.updateRotation = !strafe;
    }
}
