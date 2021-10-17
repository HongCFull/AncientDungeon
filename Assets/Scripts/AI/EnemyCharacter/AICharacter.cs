using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AICharacter : MonoBehaviour
{
    [SerializeField] private AIVision vision;
    
    [SerializeField] private float attackDistance;
    
    public float GetAttackDistance()
    {
        return attackDistance;
    }

    public bool PlayerIsInsideAttackArea()
    {
        return (PlayerCharacter.Instance.GetPlayerWorldPosition() - transform.position).magnitude  <= attackDistance;
    }

    public bool CanSeePlayer()
    {
        return vision.canSeePlayer;
    }

    public Vector3 GetAICharacterWorldPosition()
    {
        return transform.position;
    }
}
