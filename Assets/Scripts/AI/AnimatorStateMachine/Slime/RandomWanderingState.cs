using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

/// <summary>
/// 
/// </summary>
public class RandomWanderingState : StateMachineBehaviour
{
    [SerializeField] private float tolerance=0.5f;
    
    private NavMeshAgent navMeshAgent;
    private SlimeCharacter slimeCharacter;
    private Vector3 destination;
    [ReadOnly] private float distError;
        
    //AnimID
    [ReadOnly] private int hasReachedRandWanderingPosAnimID;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheReferences(animator);

        GoToRandPosition();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        UpdateAnimatorHasReachedFleePosition(animator);
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(hasReachedRandWanderingPosAnimID, false);
        StopNavmeshAgent();
    }

    private void CacheReferences(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();

        slimeCharacter = animator.gameObject.GetComponent<SlimeCharacter>();

        hasReachedRandWanderingPosAnimID = Animator.StringToHash("hasReachedRandWanderingPos");
    }

    void GoToRandPosition()
    {
        float fleeDistance = slimeCharacter.GetFleeDistance();
        Vector3 randomDistanceVec = Random.insideUnitSphere * fleeDistance;

        Vector3 targetPos = slimeCharacter.GetAICharacterWorldPosition() + randomDistanceVec;
        NavMeshHit hit;
        NavMesh.SamplePosition(targetPos, out hit,fleeDistance,1);
        destination = hit.position;
        navMeshAgent.SetDestination(destination);    
    }
    
    void UpdateAnimatorHasReachedFleePosition(Animator animator)
    {
        bool hasReachedFleePosition = AIPredicateHelper.HasReachedPosition(navMeshAgent.transform.position,destination,tolerance);
        animator.SetBool(hasReachedRandWanderingPosAnimID, hasReachedFleePosition);
    }
    
    void StopNavmeshAgent()
    {
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
    }

   
}
