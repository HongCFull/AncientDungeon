using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenAngelAttackStateMachine : AIAttackStateMachine
{
    [SerializeField] [Range(0, 1f)] private float attackAgainProbability;
    [SerializeField] [Range(0, 1f)] private float tpAfterAttackProbability;

    private int animIDAttackAgain;
    private int animIDTeleport;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animIDAttackAgain = Animator.StringToHash("attackAgain");
        animIDTeleport = Animator.StringToHash("teleport");

        ClearPreviousChoices(animator);
        DecideIfToAttackAgain(animator);
        DecideIfToTeleportAfterAttack(animator);
    }

    private void ClearPreviousChoices(Animator animator)
    {
        animator.SetBool(animIDAttackAgain,false);
        animator.SetBool(animIDTeleport,false);
    }
    
    private void DecideIfToAttackAgain(Animator animator)
    {
        float weight = Random.Range(0, 1f);
        //Debug.Log("attack weight = "+weight);
        if (weight <= attackAgainProbability)
            animator.SetBool(animIDAttackAgain,true);
    }
    
    private void DecideIfToTeleportAfterAttack(Animator animator)
    {
        float weight = Random.Range(0, 1f);
        //Debug.Log("tp weight = "+weight);
        if (weight <= tpAfterAttackProbability) 
            animator.SetBool(animIDTeleport,true);
    }
}
