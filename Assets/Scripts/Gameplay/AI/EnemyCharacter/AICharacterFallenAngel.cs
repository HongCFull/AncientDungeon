using System;
using System.Collections;
using System.Collections.Generic;
using HitBoxDefinition;
using UnityEngine;

public class AICharacterFallenAngel : AICharacter
{
    [Space]
    [Header("Unique AttackHitBox")]
    [SerializeField] private AttackHitBox leftWeaponHitBox;
    [SerializeField] private AttackHitBox rightWeaponHitBox;
    [SerializeField] private AttackHitBox attack02HitBox;
    
    [Header("Audio")]
    [SerializeField] private AudioClip attack01WeaponAudio;
    [SerializeField] private AudioClip attack02Part1WeaponAudio;
    [SerializeField] private AudioClip attack02Part2WeaponAudio;

    
    protected override void Awake()
    {
        base.Awake();
    }

    public override void DisableAllAttackHitBoxes()
    {
        DisableLeftWeaponHitBox();
        DisableRightWeaponHitBox();
        DisableAttack02HitBox();
    }

    public override void DisableAllReceiveHitBoxes()
    {
        foreach (ReceiveHitBox receiveHitBox in receiveHitBoxes)
            receiveHitBox.DisableReceiveHitBox();
    }

    public void EnableLeftWeaponHitBoxWithPower(float power)=> leftWeaponHitBox.EnableAttackColliderWithSkillPower(power);
    void DisableLeftWeaponHitBox()=> leftWeaponHitBox.DisableAttackCollider();
    public void EnableRightWeaponHitBoxWithPower(float power)=> rightWeaponHitBox.EnableAttackColliderWithSkillPower(power);
    void DisableRightWeaponHitBox()=> rightWeaponHitBox.DisableAttackCollider();
    public void EnableAttack02HitBoxWithPower(float power) => attack02HitBox.EnableAttackColliderWithSkillPower(power);
    void DisableAttack02HitBox() => attack02HitBox.DisableAttackCollider();

    void PlayAttack01WeaponAudio()
    {
        if (!attack01WeaponAudio)
            throw new Exception(gameObject.name + "'s attack01WeaponAudio is not assigned");
        
        audioSource.PlayOneShot(attack01WeaponAudio);
    } 
    void PlayAttack02Part1WeaponAudio()
    {
        if (!attack02Part1WeaponAudio)
            throw new Exception(gameObject.name + "'s attack02Part1WeaponAudio is not assigned");
        
        audioSource.PlayOneShot(attack02Part1WeaponAudio);
    }  
    void PlayAttack02Part2WeaponAudio()
    {
        if (!attack02Part2WeaponAudio)
            throw new Exception(gameObject.name + "'s attack02Part2WeaponAudio is not assigned");
        
        audioSource.PlayOneShot(attack02Part2WeaponAudio);
    } 
    
}
