using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Element;
using HitBoxDefinition;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CombatCharacterData),typeof(Animator),typeof(AudioSource))]
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

    [Header("Audio")]
    [SerializeField] private AudioClip[] footStepAudio;
    [SerializeField] private AudioClip[] damagedAudio;

    protected AudioSource audioSource;

    protected Animator animator;
    protected int animID_isDamaged;
    protected int animID_isDead;
    protected int animID_stateCanBeInterrupted;
    private bool diedOnce = false;
    
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
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
    /// Disable all the attack hit boxes (aka hurt boxes) of this combat character.
    /// Should be implemented individually
    /// </summary>
    public abstract void DisableAllAttackHitBoxes();
    
    /// <summary>
    /// Disable all the receive hit boxes of this combat character.
    /// Should be implemented individually
    /// </summary>
    public abstract void DisableAllReceiveHitBoxes();

    /// <summary>
    /// Returns true only if both HP <= 0 and animator is in the Death state. 
    /// </summary>
    public bool IsDead() => combatCharacterData.currentHealth <= 0 && animator.GetBool(animID_isDead);
    public ElementType GetElementType() => combatCharacterData.elementType;
    public float GetAttack() => combatCharacterData.attack;
    public float GetDefense() => combatCharacterData.defense;
    public float GetCurrentHealth() => combatCharacterData.currentHealth;
    public float GetMaxHealth() => combatCharacterData.maxHealth;
    public void SetCombatCharacterToInvulnerable(bool isInvulnerable) => this.invulnerable = isInvulnerable;
    
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
    
    public void PlayRandomFootStepAudio()
    {
        if (footStepAudio.Length <= 0)
            throw new Exception(gameObject.name + "'s footstep audio clips are not assigned");
        audioSource.PlayOneShot(footStepAudio[Random.Range(0,footStepAudio.Length)]);
    }
    
    /// <summary>
    /// Deal damage to this combatCharacter.
    /// Invoke events when it is damaged or dead.
    /// </summary>
    /// <param name="damage">The damage deal to this combat character </param>
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
            DisableAllReceiveHitBoxes();
            SetAnimatorIsDeadTo(true);
            whenItIsDead.Invoke();
        }
        else {

            if (animator.GetBool(animID_stateCanBeInterrupted)) {
                SetAnimationTriggerIsDamaged();
                PlayDamagedAudio();
            }
        }
    }
    
    /// <summary>
    /// Note:It shouldn't be assigned to character OnDamaged event
    /// </summary>
    void SetAnimationTriggerIsDamaged() => animator.SetTrigger(animID_isDamaged);
    
    /// <summary>
    ///  Note:It shouldn't be assigned to character OnDeath event
    /// </summary>
    void SetAnimatorIsDeadTo(bool isDead)=> animator.SetBool(animID_isDead,isDead);

    private void PlayDamagedAudio()
    {
        if (damagedAudio.Length <= 0)
            return;
            //throw new Exception(gameObject.name + "'s damaged audio clips are not assigned");
        audioSource.PlayOneShot(damagedAudio[Random.Range(0,damagedAudio.Length)]);
    }    
}
