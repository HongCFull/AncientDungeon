using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    private NavMeshAgent navMeshAgent;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheReferences(animator);
        StopNavmeshAgent();
    }

    void CacheReferences(Animator animator)
    {
        if (!navMeshAgent)
            navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
    }

    void StopNavmeshAgent()
    {
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
    }
}
