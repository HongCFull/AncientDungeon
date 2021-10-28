using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DamagedState : StateMachineBehaviour
{
    private NavMeshAgent navMeshAgent;

    private float initialAgentSpeed;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheComponents(animator);
        
        SetNavMeshAgentSpeed(0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        SetNavMeshAgentSpeed(initialAgentSpeed);

    }

    private void CacheComponents(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        initialAgentSpeed = navMeshAgent.speed;
    }

    private void SetNavMeshAgentSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    private void SetNavmeshAgentDestination(Vector3 dest)
    {
        navMeshAgent.destination = dest;
    }
    
}
