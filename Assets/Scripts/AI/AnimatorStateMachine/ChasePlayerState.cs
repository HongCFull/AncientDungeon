using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class ChasePlayerState : StateMachineBehaviour
{
    private NavMeshAgent navMeshAgent;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheReferences(animator);
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        ChasePlayer(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        StopChasing();
    }

    private void CacheReferences(Animator animator)
    {
        if(!navMeshAgent)
            navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        Debug.Log("cache navmesh agent ");
    }
    
    void ChasePlayer(Animator animator)
    {
        if (!navMeshAgent)
            CacheReferences(animator);

        navMeshAgent.SetDestination(PlayerCharacter.Instance.GetPlayerWorldPosition());
    }

    void StopChasing()
    {
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
    }
}
