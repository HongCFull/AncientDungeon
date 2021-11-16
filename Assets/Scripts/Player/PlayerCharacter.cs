using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator),typeof(AudioSource))]
public class PlayerCharacter : CombatCharacter
{
    [SerializeField] private AttackHitBox weaponHitBox;
    [SerializeField] private SlashVFXManager slashVFXManager;

    [Header("Audio")]
    [SerializeField] private AudioClip[] combo1Clips;
    [SerializeField] private AudioClip[] combo2Clips;
    [SerializeField] private AudioClip[] combo3Clips;
    [SerializeField] private AudioClip[] combo4Clips;
    
    //Audio
    private AudioSource audioSource;
    
    //Animations
    private Animator animator;
    private int isDamagedAnimID;
    
    public static PlayerCharacter Instance { get; private set; }
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
        audioSource = GetComponent<AudioSource>();
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

    public void PlayAttackAudioOfCombo(int index)
    {
        AudioClip[] atkAudioClips = null;
        switch (index) {
            case 1:
                atkAudioClips = combo1Clips;
                break;
            case 2:
                atkAudioClips = combo2Clips;
                break;
            case 3:
                atkAudioClips = combo3Clips;
                break;
            case 4:
                atkAudioClips = combo4Clips;
                break;
            default:
                break;
        }
        audioSource.Stop();
        if(atkAudioClips!=null)
            audioSource.PlayOneShot(atkAudioClips[Random.Range(0, atkAudioClips.Length)]);

    }

}
