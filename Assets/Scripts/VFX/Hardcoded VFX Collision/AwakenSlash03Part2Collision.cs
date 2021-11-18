using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakenSlash03Part2Collision : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AttackHitBox slashesHitBox;

    private int operationIndex = 0;
    private void Update()
    {
        if (particleSystem == null)
            return;
        
        if (operationIndex ==0 && particleSystem.time >0.5f) {
            operationIndex++;
            slashesHitBox.EnableAttackCollider();
        }
        else if (operationIndex==1 && particleSystem.time >0.6f) {
            operationIndex++;
            slashesHitBox.DisableAttackCollider();
        }
        else if (operationIndex == 2 && particleSystem.time > 0.7f) {
            operationIndex++;
            slashesHitBox.EnableAttackCollider();
        }
        else if (operationIndex == 3 && particleSystem.time > 0.8f) {
            operationIndex++;
            slashesHitBox.DisableAttackCollider();
        }
    }
}
