using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakenSlash04Part2Collision : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AttackHitBox slashHitBox;
    
    private void OnEnable()
    {
        slashHitBox.EnableAttackCollider();
        Invoke(nameof(DisableAttackHitBox),0.35f);
    }

    void DisableAttackHitBox()
    {
        if(slashHitBox)
            slashHitBox.DisableAttackCollider();
    }
}
