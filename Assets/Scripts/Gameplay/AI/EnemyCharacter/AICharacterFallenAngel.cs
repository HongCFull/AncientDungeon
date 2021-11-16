using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterFallenAngel : AICharacter
{
    [Space]
    [Header("Unique AttackHitBox")]
    [SerializeField] private AttackHitBox leftWeaponHitBox;
    [SerializeField] private AttackHitBox rightWeaponHitBox;
    [SerializeField] private AttackHitBox attack02HitBox;
    
    public void EnableLeftWeaponHitBoxWithPower(float power)
    {
        leftWeaponHitBox.EnableAttackColliderWithSkillPower(power);
    }

    public void DisableLeftWeaponHitBox()
    {
        leftWeaponHitBox.DisableAttackCollider();
    }

    public void EnableRightWeaponHitBoxWithPower(float power)
    {
        rightWeaponHitBox.EnableAttackColliderWithSkillPower(power);
    }

    public void DisableRightWeaponHitBox()
    {
        rightWeaponHitBox.DisableAttackCollider();
    }

    public void EnableAttack02HitBoxWithPower(float power)
    {
        attack02HitBox.EnableAttackColliderWithSkillPower(power);
    }

    public void DisableAttack02HitBox()
    {
        attack02HitBox.DisableAttackCollider();
    }
}
