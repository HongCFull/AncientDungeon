using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Element;
using HitBoxDefinition;

[RequireComponent(typeof(CombatCharacterData))]
public abstract class CombatCharacter : MonoBehaviour
{
    //Can't use get component as it other awake method depends on this data obj
    [SerializeField] CombatCharacterData combatCharacterData;
    
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
        ValidateData();
        combatCharacterData.currentHealth = Mathf.Clamp(combatCharacterData.currentHealth,0,combatCharacterData.maxHealth);
    }
    
    void ValidateData()
    {
        if (Mathf.Approximately(GetMaxHealth(), 0f)) 
            throw new System.Exception(name+" has max health of 0!");
        
        if(GetCurrentHealth()>GetMaxHealth()) 
            throw new System.Exception(name+"'s current health is greater than its max health");
        
    }

    public bool IsDead() => combatCharacterData.currentHealth <= 0;
    public ElementType GetElementType() => combatCharacterData.elementType;
    public float GetAttack() => combatCharacterData.attack;
    public float GetDefense() => combatCharacterData.defense;
    public float GetCurrentHealth() => combatCharacterData.currentHealth;
    public float GetMaxHealth() => combatCharacterData.maxHealth;
    
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
    
    public void DisableAttackHitBox(int i)
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

        bool isDead = combatCharacterData.currentHealth - damage <= 0;
        combatCharacterData.currentHealth = 
            Mathf.Clamp(MathfExtension.RoundFloatToDecimal(combatCharacterData.currentHealth-damage,1),0,GetMaxHealth());
        
        if (isDead) {
            diedOnce = true;
            whenItIsDamaged.Invoke();
            whenItIsDead.Invoke();
        }
        else {
            whenItIsDamaged.Invoke();
        }
    }
    
}
