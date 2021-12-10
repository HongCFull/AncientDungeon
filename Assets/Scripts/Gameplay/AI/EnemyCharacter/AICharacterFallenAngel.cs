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

    public void EnableLeftWeaponHitBoxWithPower(float power)
    {
        leftWeaponHitBox.EnableAttackColliderWithSkillPower(power);
    }

    void DisableLeftWeaponHitBox()
    {
        leftWeaponHitBox.DisableAttackCollider();
    }

    public void EnableRightWeaponHitBoxWithPower(float power)
    {
        rightWeaponHitBox.EnableAttackColliderWithSkillPower(power);
    }

    void DisableRightWeaponHitBox()
    {
        rightWeaponHitBox.DisableAttackCollider();
    }

    public void EnableAttack02HitBoxWithPower(float power)
    {
        attack02HitBox.EnableAttackColliderWithSkillPower(power);
    }

    void DisableAttack02HitBox()
    {
        attack02HitBox.DisableAttackCollider();
    }
}
