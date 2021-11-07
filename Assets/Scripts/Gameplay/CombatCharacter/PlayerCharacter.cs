using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class PlayerCharacter : CombatCharacter
{
    [SerializeField] private AttackHitBox weaponHitBox;
    [SerializeField] private SlashVFXManager slashVFXManager;
    public static PlayerCharacter Instance { get; private set; }

    //Animations
    private Animator animator;
    private int isDamagedAnimID;
    
    public SlashVFXManager GetSlashVFXManager() => slashVFXManager;
    
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        InitializeVariables();
    }
    
    void InitializeVariables()
    {
        if (!Instance)
            Instance = this;

        animator = GetComponent<Animator>();
        isDamagedAnimID = Animator.StringToHash("IsDamaged");
    }
    
    public Vector3 GetPlayerWorldPosition()
    {
        return transform.position;
    }

    public void SetAnimationTriggerIsDamaged()
    {
        animator.SetTrigger(isDamagedAnimID);
        //Debug.Log("Trigger is damaged");
    }

    public void EnableAttackHitBoxOfWeapon()
    {
        weaponHitBox.EnableAttackCollider();
    }
    
    public void EnableAttackHitBoxOfWeaponWithPower(float skillPower)
    {
        weaponHitBox.EnableAttackColliderWithSkillPower(skillPower);
    }
    
    public void DisableAttackHitBoxOfWeapon()
    {
        weaponHitBox.DisableAttackCollider();
    }
    
    
}
