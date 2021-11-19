using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Element;
using HitBoxDefinition;

public abstract class CombatCharacter : MonoBehaviour
{
    
    [Header("Character Combat Settings")]
    [SerializeField] private float attack;
    [SerializeField] private float defense;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private ElementType elementType;
    public bool canBeDamaged = true;
    
    [Tooltip("when it is damaged and survived afterward")]
    [SerializeField] private UnityEvent whenItIsDamaged;
    
    [SerializeField] private UnityEvent whenItIsDead;

    [Header("HitBoxes")]
    [SerializeField] protected List<ReceiveHitBox> receiveHitBoxes;
    [SerializeField] protected List<AttackHitBox> attackHitBoxes;

    private bool diedOnce = false;
    
    protected virtual void Awake()
    {
        //Debug.Log("Damageable::Awake");
        currentHealth = Mathf.Clamp(currentHealth,0,maxHealth);
    }
    
    public bool IsDead() => currentHealth <= 0;
    
    public ElementType GetElementType() => elementType;
    public float GetAttack() => attack;
    public float GetDefense() => defense;
    public float GetCurrentHealth() =>currentHealth;
    public float GetMaxHealth() => maxHealth;
    
    public void DisableAllAttackHitBoxes()
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
    /// <summary>
    /// Deal damage to this damageable gameObject.
    /// Invoke events when it is damaged or dead.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamageBy(float damage)
    {
        if( diedOnce || !canBeDamaged )
            return;

        currentHealth -= damage;
        
        if (currentHealth <= 0 ) {
            diedOnce = true;
            whenItIsDead.Invoke();
        }
        else {
            whenItIsDamaged.Invoke();
        }
    }
    
}
