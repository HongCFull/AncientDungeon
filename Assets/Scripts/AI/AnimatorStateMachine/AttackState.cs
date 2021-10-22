using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    [SerializeField] private int numOfAttack;
    
    private NavMeshAgent navMeshAgent;
    private int animIDAttackIndex;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
        animator.SetInteger(animIDAttackIndex,Random.Range(0,numOfAttack));
    }
}
