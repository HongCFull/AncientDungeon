using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class FleeToRandomPositionState : StateMachineBehaviour
{
    [SerializeField] private float tolerance ;
    
    private NavMeshAgent navMeshAgent;
    private SlimeCharacter slimeCharacter;
    private Vector3 destination;
    [ReadOnly] private float distError;
        
    //AnimID
    [ReadOnly] private int hasReachedFleePosAnimID;
    
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

        if (HasReachedFleePosition()) {
            StopNavmeshAgent();
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(hasReachedFleePosAnimID, false);
        StopNavmeshAgent();
    }

    private void CacheReferences(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();

        slimeCharacter = animator.gameObject.GetComponent<SlimeCharacter>();

        hasReachedFleePosAnimID = Animator.StringToHash("hasReachedFleePos");
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

    bool HasReachedFleePosition()
    {
        distError = (navMeshAgent.transform.position - destination).magnitude;
        return distError <= tolerance;
    }
    
    void UpdateAnimatorHasReachedFleePosition(Animator animator)
    {
        animator.SetBool(hasReachedFleePosAnimID, HasReachedFleePosition());
    }
    
    void StopNavmeshAgent()
    {
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
    }

   
}
