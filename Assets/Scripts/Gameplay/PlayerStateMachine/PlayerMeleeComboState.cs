using System.Collections;
using System.Collections.Generic;
using TPSTemplate;
using UnityEngine;

public class PlayerMeleeComboState : StateMachineBehaviour
{
    [Tooltip("It can trigger multiple vfx in one state")]
    [SerializeField] private int[] slashVFXIndexs;

    [SerializeField] private bool enableRootMotion;
    [SerializeField] private bool vfxMoveWithPlayer;

    [Header("VFX move with player settings")] 
    [SerializeField] [Range(-0.1f,5f)] private float appendDuration;
    
    private PlayerCharacter playerCharacter;
    private bool originalRMOption;
    private bool hasRotated;
    private bool hasExited;
    
    //Animation IDs
    private int animIDIsInComboState;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        CacheComponents(animator);
        SetRootMotionTo(animator,enableRootMotion);
        animator.SetBool(animIDIsInComboState,true);
        SpawnSlashVFX(animator);

    }

    
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //State move will still be called after the state has exit!
        if (hasExited) 
            return;
        
        base.OnStateMove(animator, stateInfo, layerIndex);
        if (!hasRotated) {
            RotatePlayerFocus(animator);
            SetRootMotionTo(animator,enableRootMotion);
        }
        else {
            animator.gameObject.transform.position = animator.rootPosition;
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        hasExited = true;
        animator.SetBool(animIDIsInComboState,false);
        animator.applyRootMotion = originalRMOption;
        playerCharacter.DisableAttackHitBoxOfWeapon();
        hasRotated = false;
    }

    private void CacheComponents(Animator animator)
    {
        playerCharacter = animator.gameObject.GetComponent<PlayerCharacter>();
        animIDIsInComboState = Animator.StringToHash("IsInComboState");
        originalRMOption = animator.applyRootMotion;
        
        hasRotated = false;
        hasExited = false;
    }

    private void RotatePlayerFocus(Animator animator)
    {
        SetRootMotionTo(animator, false);
        playerCharacter.transform.LookAt(playerCharacter.GetPlayerWorldPosition()+PlayerCamera.Instance.GetUnitForwardVectorInXZPlane());
        hasRotated = true;
    }

    private void SetRootMotionTo(Animator animator,bool applyRM)
    {
        animator.applyRootMotion = applyRM;
    }
    
   
    void SpawnSlashVFX(Animator animator) {
        for (int i = 0; i<slashVFXIndexs.Length; i++)
            if(!vfxMoveWithPlayer)
                PlayerCharacter.Instance.GetSlashVFXManager().SpawnSlashEffect(slashVFXIndexs[i]);
            else
                PlayerCharacter.Instance.GetSlashVFXManager().SpawnSlashEffectThatFollowsPlayer(slashVFXIndexs[i],appendDuration);

    }
}
