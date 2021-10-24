using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AICharacter : MonoBehaviour
{
    [SerializeField] private AIVision vision;
    [SerializeField] private float attackDistance;
    
    [Header("Debug Settings")] 
    [SerializeField] private bool showAttackDistance;
    [SerializeField] private Color attackDistanceColor;

    public Vector3 initPosition { get; private set; }

    private void Awake()
    {
        initPosition = transform.position;
        DisableAllAttackColliders();
    }

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

    protected abstract void DisableAllAttackColliders();
    protected abstract void EnableAttackCollider(int i);
    protected abstract void DisableAttackCollider(int i);

    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showAttackDistance) {
            Gizmos.color = attackDistanceColor;
            Gizmos.DrawWireSphere(transform.position,attackDistance);
        }
        
    }
#endif
    
}
