using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DamagedState : StateMachineBehaviour
{
    private NavMeshAgent navMeshAgent;
    private int animID_isHatred;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheComponents(animator);
        
        SetNavmeshAgentDestination(navMeshAgent.transform.position);
        animator.SetBool(animID_isHatred,true);
    }
    

    private void CacheComponents(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        animID_isHatred = Animator.StringToHash("isHatred");
    }
    
    private void SetNavmeshAgentDestination(Vector3 dest)
    {
        navMeshAgent.destination = dest;
    }
    
}
