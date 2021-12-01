using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttackState : ArtificialGravityState
{

    [FormerlySerializedAs("playerAttackStatus")]
    [Header("Choosing the Layer" )]
    [Tooltip("When the attack status of the player, prevent invoking this state from different layers at the same time ")]
    [SerializeField] private PlayerAttackMode playerAttackMode;
   
    [Header("RootMotion Settings")]
    [SerializeField] private bool enableRootMotion;

    [Tooltip("The frame that starts playing root motion in this state")]
    [SerializeField] [Range(0,100)] private int startingRMFrame; 
    
    private PlayerCharacter playerCharacter;
    private ThirdPersonController thirdPersonController;
    
    private bool originalRMOption;
    private bool hasRotated;
    private bool hasExited;

    private const int awakenLayerIndex = 1; 
    
    //Animation frame calculations
    //private AnimatorClipInfo[] animClipInfo;
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CacheComponents(animator,layerIndex);
        
        if (!IsInCalledInTheRightLayer(animator))
            return;
            
        // Debug.Log( "status at " + playerAttackStatus+" with length = "+ stateInfo.length);

        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        thirdPersonController.DisableCharacterWalkingForBaseLayer();
        
        SetRootMotionTo(animator,enableRootMotion);
    }

    
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!IsInCalledInTheRightLayer(animator))
            return;
        
        //State move will still be called after the state has exit!
        if (hasExited) 
            return;
        
        if (!hasRotated) {
            RotatePlayerFocus(animator);
            SetRootMotionTo(animator,enableRootMotion);
        }
        else {
            animator.ApplyBuiltinRootMotion();

         //   if (GetCurrentFrame(stateInfo) >= startingRMFrame)
            base.OnStateMove(animator, stateInfo, layerIndex);
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!IsInCalledInTheRightLayer(animator))
            return;
        
        base.OnStateExit(animator, stateInfo, layerIndex);
        
       // animator.SetBool(animIDIsInComboState,false);
       
       // animator.applyRootMotion = originalRMOption;
        playerCharacter.ForceDisableAttackHitBoxOfWeapon();
       // thirdPersonController.ForceEnableCharacterWalking();
        hasExited = true;
        hasRotated = false;
    }

    private void CacheComponents(Animator animator,int layerIndex)
    {
        //Debug.Log("Enter attack state with layer index = "+layerIndex);
        playerCharacter = animator.GetComponent<PlayerCharacter>();
        thirdPersonController = animator.GetComponent<ThirdPersonController>();
        //animClipInfo = animator.GetCurrentAnimatorClipInfo(layerIndex);

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
    
    bool IsInCalledInTheRightLayer(Animator animator)
    {
        
        if (playerCharacter.AwakenLayerIsActive() && playerAttackMode == PlayerAttackMode.AWAKEN)
            return true;

        if (!playerCharacter.AwakenLayerIsActive() && playerAttackMode == PlayerAttackMode.NORMAL)
            return true;
        
        return false;
    }
}
/*
    int GetCurrentFrame(AnimatorStateInfo stateInfo)
    {
        if (animClipInfo.Length <= 0)
            return -1;
        float frame= animClipInfo[0].clip.length * (stateInfo.normalizedTime % 1) * animClipInfo[0].clip.frameRate;
        return (int) frame;
    }
*/
