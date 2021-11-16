using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerAttackState : ArtificialGravityState
{

    [Header("RootMotion Settings")]
    [SerializeField] private bool enableRootMotion;

   // [SerializeField] private bool applyGravityInRM;
    [Tooltip("The frame that starts playing root motion in this state")]
    [SerializeField] [Range(0,100)]private int startingRMFrame; 

    [Header("VFX settings")] 
    [Tooltip("It can trigger multiple vfx in one state")]
    [SerializeField] private int[] slashVFXIndexs;
    [SerializeField] private bool vfxMoveWithPlayer;
    [SerializeField] [Range(-0.1f,5f)] private float appendDuration;
    
    private PlayerCharacter playerCharacter;
    //private CharacterController characterController;
    private ThirdPersonController thirdPersonController;
    
    private bool originalRMOption;
    private bool hasRotated;
    private bool hasExited;
    private bool hasEnter = false;

    //Animation frame calculations
    private AnimatorClipInfo[] animClipInfo;
    
    //Animation IDs
   // private int animIDIsInComboState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        CacheComponents(animator);
     //   animator.SetBool(animIDIsInComboState,true);
        
        thirdPersonController.DisableCharacterWalking();
        
        SetRootMotionTo(animator,enableRootMotion);
        SpawnSlashVFX();
    }

    
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //State move will still be called after the state has exit!
        if (hasExited) 
            return;
        
        if (!hasRotated) {
            RotatePlayerFocus(animator);
            SetRootMotionTo(animator,enableRootMotion);
        }
        else {
            animator.ApplyBuiltinRootMotion();

//            if (applyGravityInRM && GetCurrentFrame(stateInfo) >= startingRMFrame) {
  //              characterController.Move(new Vector3(0, -15f, 0f) * Time.deltaTime);
            if (GetCurrentFrame(stateInfo) >= startingRMFrame)
                base.OnStateMove(animator, stateInfo, layerIndex);
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
       // animator.SetBool(animIDIsInComboState,false);
        
        animator.applyRootMotion = originalRMOption;
        playerCharacter.DisableAttackHitBoxOfWeapon();
        thirdPersonController.EnableCharacterWalking();

        hasExited = true;
        hasRotated = false;
        hasEnter = false;
    }

    private void CacheComponents(Animator animator)
    {
        playerCharacter = animator.GetComponent<PlayerCharacter>();
//        characterController = animator.GetComponent<CharacterController>();
        thirdPersonController = animator.GetComponent<ThirdPersonController>();
        
      //  animIDIsInComboState = Animator.StringToHash("IsInComboState");
        
        animClipInfo = animator.GetCurrentAnimatorClipInfo(0);

        originalRMOption = animator.applyRootMotion;
        
        hasRotated = false;
        hasExited = false;
        hasEnter = true;
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
    
   
    void SpawnSlashVFX() 
    {
        for (int i = 0; i<slashVFXIndexs.Length; i++)
            if(!vfxMoveWithPlayer)
                PlayerCharacter.Instance.GetSlashVFXManager().SpawnSlashEffect(slashVFXIndexs[i]);
            else
                PlayerCharacter.Instance.GetSlashVFXManager().SpawnSlashEffectThatFollowsPlayer(slashVFXIndexs[i],appendDuration);
    }

    int GetCurrentFrame(AnimatorStateInfo stateInfo)
    {
        if (animClipInfo.Length <= 0)
            return -1;
        float frame= animClipInfo[0].clip.length * (stateInfo.normalizedTime % 1) * animClipInfo[0].clip.frameRate;
        return (int) frame;
    }
}
