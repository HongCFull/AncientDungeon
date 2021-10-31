using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    [SerializeField] private int numOfAttack;
    
    private NavMeshAgent navMeshAgent;
    private AICharacter aiCharacter;
    private int animIDAttackIndex;
    private int randAttackIndex;
    
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
        aiCharacter = animator.gameObject.GetComponent<AICharacter>();
        animIDAttackIndex = Animator.StringToHash("attackIndex");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        //To prevent the reset in animation event is not being called
        aiCharacter.DisableAttackHitBox(randAttackIndex);
    }

    void StopNavmeshAgent()
    {
        navMeshAgent.SetDestination(navMeshAgent.transform.position);
    }

    void SetAnimatorIntAttackIndex(Animator animator)
    {
        randAttackIndex = Random.Range(0, numOfAttack);
        animator.SetInteger(animIDAttackIndex,randAttackIndex);
    }
}
