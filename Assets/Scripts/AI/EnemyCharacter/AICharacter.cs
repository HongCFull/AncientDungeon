using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public abstract class AICharacter : Damageable
{
    
    [Header("Perception Settings")]
    [SerializeField] private AIVision vision;

    [Header("Attack Settings")] 
    [SerializeField] private float attackDistance;
    [SerializeField] private List<AttackHitBox> attackHitBoxes;
    
    [Header("Debug Settings")] 
    [SerializeField] private bool showAttackDistance;
    [SerializeField] private Color attackDistanceColor;
    
    //Animation
    private Animator animator;
    private int isDamagedAnimID;
    
    public Vector3 initPosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        //Debug.Log("AICharacter Awake");
        
        
        //DisableAllAttackHitBoxes();
    }

    void InitializeVariables()
    {
        initPosition = transform.position;
        animator = GetComponent<Animator>();
        isDamagedAnimID = Animator.StringToHash("IsDamaged");
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

    protected void DisableAllAttackHitBoxes()
    {
        foreach (AttackHitBox attackHitBox in attackHitBoxes) {
            attackHitBox.DisableAttackCollider();
        }
    }
    
    protected void EnableAttackHitBox(int i)
    {
        if (i < 0 || i >= attackHitBoxes.Count)
            return;
        
        attackHitBoxes[i].EnableAttackCollider();
    }
    
    protected void DisableAttackHitBox(int i)
    {
        if (i < 0 || i >= attackHitBoxes.Count)
            return;
        
        attackHitBoxes[i].DisableAttackCollider();
    }

    public void SetAnimationTriggerIsDamaged()
    {
        animator.SetTrigger(isDamagedAnimID);
    }
    
    
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
