using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackStateMachine : StateMachineBehaviour
{
    [SerializeField] private int numOfAttack;
    
    private NavMeshAgent navMeshAgent;
    private int animIDAttackIndex;
    private int randAttackIndex;
    
    public new virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheReferences(animator);
        StopNavmeshAgent();
        SetAnimatorIntAttackIndex(animator);
    }
    

    void CacheReferences(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        animIDAttackIndex = Animator.StringToHash("attackIndex");
    }


    void StopNavmeshAgent()
    {
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
    }

    void SetAnimatorIntAttackIndex(Animator animator)
    {
        randAttackIndex = Random.Range(0, numOfAttack);
        animator.SetInteger(animIDAttackIndex,randAttackIndex);
        //Debug.Log("enter attack index = "+randAttackIndex);
    }
}
