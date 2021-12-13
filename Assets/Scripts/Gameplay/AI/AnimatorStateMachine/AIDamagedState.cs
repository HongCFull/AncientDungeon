using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

public class AIDamagedState : StateMachineBehaviour
{
    [SerializeField] private float knockBackDistance;
    protected bool enableManualFallBack=true;
    private NavMeshAgent navMeshAgent;
    private int animID_isHatred;
    private bool hasExit;
    public new virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Initialize(animator);
        
        SetNavmeshAgentDestination(navMeshAgent.transform.position);
        animator.SetBool(animID_isHatred,true);
    }

    public new virtual void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasExit || !enableManualFallBack)
            return;
        
        base.OnStateMove(animator, stateInfo, layerIndex);
        Vector3 movementDir = (navMeshAgent.transform.position - PlayerCharacter.Instance.GetPlayerWorldPosition())
            .normalized;
        Vector3 movement = movementDir * knockBackDistance + Vector3.down*-15f;
        
        navMeshAgent.Move(movement*Time.deltaTime);
    }

    public new virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        hasExit = true;
    }

    protected void Initialize(Animator animator)
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
