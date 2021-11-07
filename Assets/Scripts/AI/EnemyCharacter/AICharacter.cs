using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using HitBoxDefinition;
[RequireComponent(typeof(Animator))]
public abstract class AICharacter : CombatCharacter
{
    
    [Header("Perception Settings")]
    [SerializeField] private AIVision vision;

    [Header("Attack Settings")] 
    [SerializeField] private float attackDistance;
    [SerializeField] private List<AttackHitBox> attackHitBoxes;
    
    [Header("Debug Settings")] 
    [SerializeField] private bool showAttackDistance;
    [SerializeField] private Color attackDistanceColor;

    [Header("Animation Settings")]
    [SerializeField] private float deathDelay;

    [Header("Mesh")] 
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    
    //Animation
    private Animator animator;
    private int animID_isDamaged;
    private int animID_isDead;
    private int animID_isInvulnerable;
    
    
    public Vector3 initPosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        //Debug.Log("AICharacter Awake");
        
        InitializeVariables();
        //DisableAllAttackHitBoxes();
    }

    void InitializeVariables()
    {
        initPosition = transform.position;
        animator = GetComponent<Animator>();
        
        animID_isDamaged = Animator.StringToHash("isDamaged");
        animID_isDead = Animator.StringToHash("isDead");
        animID_isInvulnerable = Animator.StringToHash("isInvulnerable");
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

    public void DisableAllAttackHitBoxes()
    {
        foreach (AttackHitBox attackHitBox in attackHitBoxes) {
            attackHitBox.DisableAttackCollider();
        }
    }
    
    public void EnableAttackHitBox(int i)
    {
        if (i < 0 || i >= attackHitBoxes.Count)
            return;
        
        attackHitBoxes[i].EnableAttackCollider();
    }
    
    public void DisableAttackHitBox(int i)
    {
        if (i < 0 || i >= attackHitBoxes.Count)
            return;
        
        attackHitBoxes[i].DisableAttackCollider();
    }

    public void SetAnimationTriggerIsDamaged()
    {
        if(!animator.GetBool(animID_isInvulnerable))
            animator.SetTrigger(animID_isDamaged);
    }

    public void OperationWhenItIsDead()
    {
        DisableAllReceiveHitBoxes();
        SetAnimationTriggerIsDead();
        //Destroy(gameObject,deathDelay);
    }


    private void DisableAllReceiveHitBoxes()
    {
        foreach (ReceiveHitBox receiveHitBox in registeredHitBoxes) {
            receiveHitBox.EnableHitBox(false);
        }    
    }
    
    void SetAnimationTriggerIsDead()
    {
        animator.SetTrigger(animID_isDead);
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
