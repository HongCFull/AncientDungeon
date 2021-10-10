using System.Collections;
using System.Collections.Generic;
using TPSTemplate;
using UnityEngine;

public class EnterMeleeComboState : StateMachineBehaviour
{
    [SerializeField] private int slashVFXIndex;
    private ThirdPersonController tpsController = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        ResetMeleeAttack(animator);
        SpawnSlashVFX(animator);
       // Debug.Log("Enter melee combo state");
    }

    void ResetMeleeAttack(Animator animator) {
        animator.ResetTrigger("MeleeAttack");
        //Debug.Log("Reset MeleeAttack");   
    }

    void SpawnSlashVFX(Animator animator) {
        if(!tpsController )
            tpsController = animator.GetComponent<ThirdPersonController>(); 
        
        tpsController.slashVFXManager.SpawnSlashEffect(slashVFXIndex);
       

    }
}
