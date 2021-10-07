using System;
using System.Collections;
using System.Collections.Generic;
using TPSTemplate;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(ThirdPersonController))]
public class MeleeComboHandler : MonoBehaviour
{
    private Animator animator = null;
    private ThirdPersonController thirdPersonController;
    private int animIDCanTriggerNextCombo;
    private void OnEnable()
    {
        if (!animator) {
            animator = GetComponent<Animator>();
            animIDCanTriggerNextCombo = Animator.StringToHash("CanTriggerNextCombo");
        }

        if (!thirdPersonController) {
            thirdPersonController = GetComponent<ThirdPersonController>();
        }
    }

    private void InitializeComboSetting() {
        ResetCanTriggerNextCombo();
        thirdPersonController.DisableCharacterMovement();
    }
    /*
    private void AllowNextComboTransition() {
        SetCanTriggerNextCombo();
        thirdPersonController.EnableCharacterMovement();
    }

    private void FinishComboSetting() {
        ResetCanTriggerNextCombo();
        thirdPersonController.EnableCharacterMovement();
    }*/
    
    private void SetCanTriggerNextCombo() {
        animator.SetTrigger(animIDCanTriggerNextCombo);
    }
    
    private void ResetCanTriggerNextCombo() {
        animator.ResetTrigger(animIDCanTriggerNextCombo);
    }
}
