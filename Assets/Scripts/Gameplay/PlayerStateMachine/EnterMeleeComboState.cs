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

    private PlayerCharacter playerCharacter;
    
    //Animation IDs
    private int animIDIsInComboState;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        CacheComponents(animator);
        
        animator.SetBool(animIDIsInComboState,true);
        EnableRootMotion(animator);
        SpawnSlashVFX(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(animIDIsInComboState,false);
        playerCharacter.DisableAttackHitBoxOfWeapon();
    }

    private void CacheComponents(Animator animator)
    {
        playerCharacter = animator.gameObject.GetComponent<PlayerCharacter>();
        animIDIsInComboState = Animator.StringToHash("IsInComboState");

    }

    private void EnableRootMotion(Animator animator)
    {
        animator.applyRootMotion = enableRootMotion;
    }
    
   
    void SpawnSlashVFX(Animator animator) {
        for (int i = 0; i<slashVFXIndexs.Length; i++)
            PlayerCharacter.Instance.GetSlashVFXManager().SpawnSlashEffect(slashVFXIndexs[i],vfxMoveWithPlayer);

    }
}
