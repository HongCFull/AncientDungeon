using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookAtPlayerState : StateMachineBehaviour
{
    private NavMeshAgent navMeshAgent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheReferences(animator);
        navMeshAgent.updateRotation = false;
        LookAtPlayerImmediately(animator);
        navMeshAgent.updateRotation = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
       // LookAtPlayerImmediately();

    }

    private void CacheReferences(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
    }
    
    void LookAtPlayerImmediately(Animator animator)
    {
        animator.enabled = false;
        navMeshAgent.transform.LookAt(PlayerCharacter.Instance.transform);
        animator.enabled = true;
Debug.Log("look at player");
//        navMeshAgent.transform.LookAt(PlayerCharacter.Instance.transform);
    }
}
