using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

public class AIDamagedState : StateMachineBehaviour
{
    [SerializeField] private float knockBackDistance;
    private NavMeshAgent navMeshAgent;
    private int animID_isHatred;
    private bool hasExit;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Initialize(animator);
        
        SetNavmeshAgentDestination(navMeshAgent.transform.position);
        animator.SetBool(animID_isHatred,true);
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasExit)
            return;
        
        base.OnStateMove(animator, stateInfo, layerIndex);
        Vector3 movement = -1 * navMeshAgent.transform.forward.normalized * knockBackDistance + Vector3.down*-15f;
        
        navMeshAgent.Move(movement*Time.deltaTime);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        hasExit = true;
    }

    private void Initialize(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        animID_isHatred = Animator.StringToHash("isHatred");
        hasExit = false;
    }
    
    private void SetNavmeshAgentDestination(Vector3 dest)
    {
        navMeshAgent.destination = dest;
    }
    
}
