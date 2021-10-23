using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class ChasePlayerState : StateMachineBehaviour
{
    private NavMeshAgent navMeshAgent;
    private AICharacter aiCharacter;
    
    //Anim id
    private int hasBeenResetAnimID;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheReferences(animator);
        LookAtPlayer(animator);
        
        animator.SetBool(hasBeenResetAnimID,false);
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        ChasePlayer(animator);
        
        if(aiCharacter.PlayerIsInsideAttackArea())
            StopChasing();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

    private void CacheReferences(Animator animator)
    {
        navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        aiCharacter = animator.gameObject.GetComponent<AICharacter>();

        hasBeenResetAnimID = Animator.StringToHash("hasBeenReset");
    }

    private void LookAtPlayer(Animator animator)
    {
        navMeshAgent.transform.LookAt(PlayerCharacter.Instance.GetPlayerWorldPosition());
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
