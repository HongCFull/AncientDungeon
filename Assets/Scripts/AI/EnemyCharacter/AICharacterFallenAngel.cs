using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterFallenAngel : AICharacter
{
    [Space]
    [Header("Unique AttackHitBox")]
    [SerializeField] private AttackHitBox leftWeaponHitBox;
    [SerializeField] private AttackHitBox rightWeaponHitBox;

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
}
