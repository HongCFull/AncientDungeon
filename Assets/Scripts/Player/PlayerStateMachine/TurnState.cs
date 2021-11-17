using System.Collections;
using System.Collections.Generic;

using Player;
using UnityEngine;

public class TurnState : StateMachineBehaviour
{

    private PlayerCharacter playerCharacter;
    private ThirdPersonController thirdPersonController;
    private bool originalRMOption;
    private bool hasRotated;
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheComponents(animator);

        thirdPersonController.ForceDisableCharacterWalking();
        
        animator.applyRootMotion = false;
        RotatePlayerFocus(animator);
        
    }

    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        animator.applyRootMotion = originalRMOption;
        thirdPersonController.ForceEnableCharacterWalking();

    }

    private void CacheComponents(Animator animator)
    {
        playerCharacter = animator.GetComponent<PlayerCharacter>();
        thirdPersonController = animator.GetComponent<ThirdPersonController>();
        
        originalRMOption = animator.applyRootMotion;
        
    }

    private void RotatePlayerFocus(Animator animator)
    {
        playerCharacter.transform.LookAt(playerCharacter.GetPlayerWorldPosition()+PlayerCamera.Instance.GetUnitForwardVectorInXZPlane());
    }
    
    

}
