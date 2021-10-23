using System.Collections;
using System.Collections.Generic;
using TPSTemplate;
using UnityEngine;

public class EnterMeleeComboState : StateMachineBehaviour
{
    [Tooltip("It can trigger multiple vfx in one state")]
    [SerializeField] private int[] slashVFXIndexs;

    [SerializeField] private bool enableRootMotion;
    [SerializeField] private bool vfxMoveWithPlayer;
    private ThirdPersonController tpsController = null;
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        EnableRootMotion(animator);
        ResetMeleeAttack(animator);
        SpawnSlashVFX(animator);
        Debug.Log("Enter melee combo state");
    }

    private void EnableRootMotion(Animator animator)
    {
        animator.applyRootMotion = enableRootMotion;
    }
    
    void ResetMeleeAttack(Animator animator) {
        animator.ResetTrigger("MeleeAttack");
        //Debug.Log("Reset MeleeAttack");   
    }

    void SpawnSlashVFX(Animator animator) {
        /*
        if(!tpsController )
            tpsController = animator.GetComponent<ThirdPersonController>(); 
        
        for (int i = 0; i<slashVFXIndexs.Length; i++)
            tpsController.slashVFXManager.SpawnSlashEffect(slashVFXIndexs[i],vfxMoveWithPlayer);
       */
        for (int i = 0; i<slashVFXIndexs.Length; i++)
            PlayerCharacter.Instance.GetSlashVFXManager().SpawnSlashEffect(slashVFXIndexs[i],vfxMoveWithPlayer);

    }
}
