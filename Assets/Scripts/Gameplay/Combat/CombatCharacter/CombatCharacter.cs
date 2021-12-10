using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;
using UnityEngine.Events;
using Element;
using HitBoxDefinition;

[RequireComponent(typeof(CombatCharacterData),typeof(Animator))]
public abstract class CombatCharacter : MonoBehaviour
{
    //Can't use get component as it other awake method depends on this data obj
    [SerializeField] CombatCharacterData combatCharacterData;
    
    protected bool invulnerable = false;
    
    [Tooltip("when it is damaged and survived afterward")]
    [SerializeField] private UnityEvent whenItIsDamaged;
    
    [SerializeField] private UnityEvent whenItIsDead;

    [Header("HitBoxes")]
    [SerializeField] protected List<ReceiveHitBox> receiveHitBoxes;
    [SerializeField] protected List<AttackHitBox> attackHitBoxes;

    protected Animator animator;
    protected int animID_isDamaged;
    protected int animID_isDead;
    protected int animID_stateCanBeInterrupted;
    
    private bool diedOnce = false;
    
    protected virtual void Awake()
    {

        animator = GetComponent<Animator>();
        animID_isDamaged = Animator.StringToHash("isDamaged");
        animID_isDead = Animator.StringToHash("isDead");
        animID_stateCanBeInterrupted = Animator.StringToHash("stateCanBeInterrupted");

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

    /// <summary>
    /// Returns true only if both HP <= 0 and it is in the Death state in animator. 
    /// </summary>
    /// <returns></returns>
    public bool IsDead() => combatCharacterData.currentHealth <= 0 && animator.GetBool(animID_isDead);
    public ElementType GetElementType() => combatCharacterData.elementType;
    public float GetAttack() => combatCharacterData.attack;
    public float GetDefense() => combatCharacterData.defense;
    public float GetCurrentHealth() => combatCharacterData.currentHealth;
    public float GetMaxHealth() => combatCharacterData.maxHealth;
    public void SetCombatCharacterToInvulnerable(bool isInvulnerable) => this.invulnerable = isInvulnerable;

    /// <summary>
    /// Disable all the attack hit boxes of this combat character.
    /// Should be implemented individually
    /// </summary>
    public abstract void DisableAllAttackHitBoxes();

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
    /// Deal damage to this combatCharacter.
    /// Invoke events when it is damaged or dead.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamageBy(float damage)
    {
        if( diedOnce || invulnerable )
            return;

        bool isDead = combatCharacterData.currentHealth - damage <= 0;
        combatCharacterData.currentHealth = 
            Mathf.Clamp(MathfExtension.RoundFloatToDecimal(combatCharacterData.currentHealth-damage,1),0,GetMaxHealth());

        whenItIsDamaged.Invoke();
        
        if (isDead) {
            diedOnce = true;
            SetAnimatorIsDeadTo(true);
            whenItIsDead.Invoke();
        }
        else {
            SetAnimationTriggerIsDamaged();
        }
    }
    
    /// <summary>
    /// Note:It shouldn't be assigned to character OnDamaged event
    /// </summary>
    void SetAnimationTriggerIsDamaged()
    {
        if(animator.GetBool(animID_stateCanBeInterrupted))
            animator.SetTrigger(animID_isDamaged);
    }

    /// <summary>
    ///  Note:It shouldn't be assigned to character OnDeath event
    /// </summary>
    void SetAnimatorIsDeadTo(bool isDead)
    {
        animator.SetBool(animID_isDead,isDead);
    }
    
    
}
