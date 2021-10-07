using System;
using System.Collections;
using System.Collections.Generic;
using TPSTemplate;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(ThirdPersonController))]
public class MeleeComboHandler : MonoBehaviour
{
    private Animator animator = null;
    private int animIDCanTriggerNextCombo;
    private void OnEnable()
    {
        if (!animator) {
            animator = GetComponent<Animator>();
            animIDCanTriggerNextCombo = Animator.StringToHash("CanTriggerNextCombo");
        }
    }

    private void SetCanTriggerNextCombo() {
        animator.SetTrigger(animIDCanTriggerNextCombo);
    }
    
    private void ResetCanTriggerNextCombo() {
        animator.ResetTrigger(animIDCanTriggerNextCombo);
    }
}
