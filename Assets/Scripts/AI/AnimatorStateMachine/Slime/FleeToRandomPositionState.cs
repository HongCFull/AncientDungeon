using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeToRandomPositionState : StateMachineBehaviour
{
    private NavMeshAgent navMeshAgent;
    private SlimeCharacter slimeCharacter;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheReferences(animator);

        GoToRandPosition();
    }

    private void CacheReferences(Animator animator)
    {
        if(!navMeshAgent)
            navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();

        if (!slimeCharacter)
            slimeCharacter = animator.gameObject.GetComponent<SlimeCharacter>();
    }

    void GoToRandPosition()
    {
        float fleeDistance = slimeCharacter.GetFleeDistance();
        Vector3 randomDistanceVec = Random.insideUnitSphere * fleeDistance;

        Vector3 targetPos = slimeCharacter.GetAICharacterWorldPosition() + randomDistanceVec;
        NavMeshHit hit;
        NavMesh.SamplePosition(targetPos, out hit,fleeDistance,1);
        
        navMeshAgent.SetDestination(hit.position);    
    }
    
}
