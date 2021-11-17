using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;


public enum PlayerAttackStatus
{
    NORMAL, AWAKEN
}

[RequireComponent(typeof(ThirdPersonController),typeof(Animator),typeof(AudioSource))]
public class PlayerCharacter : CombatCharacter
{
    [SerializeField] private AttackHitBox weaponHitBox;
    [SerializeField] private SlashVFXManager slashVFXManager;

    [Header("Audio")]
    [SerializeField] private AudioClip[] combo1Clips;
    [SerializeField] private AudioClip[] combo2Clips;
    [SerializeField] private AudioClip[] combo3Clips;
    [SerializeField] private AudioClip[] combo4Clips;

    public PlayerAttackStatus playerAttackStatus;
    
    //Audio
    private AudioSource audioSource;
    
    //Animations
    private Animator animator;
    private int animID_IsDamaged;
    private int animID_CanTriggerNextCombo;
    private const int AwakenLayerIndex = 1;
    
    //TPS controller
    private ThirdPersonController thirdPersonController;

    
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
        thirdPersonController = GetComponent<ThirdPersonController>();

        animID_CanTriggerNextCombo = Animator.StringToHash("CanTriggerNextCombo");
        animID_IsDamaged = Animator.StringToHash("IsDamaged");
    }
    
    public Vector3 GetPlayerWorldPosition()
    {
        return transform.position;
    }

    public void SetAnimationTriggerIsDamaged()
    {
        animator.SetTrigger(animID_IsDamaged);
        //Debug.Log("Trigger is damaged");
    }

    #region ComboHandling

    /// <summary>
    /// Used as the animation event callback of player's attack combo
    /// </summary>

        public bool AwakenLayerIsActive()
        {
            return (Mathf.Approximately(animator.GetLayerWeight(AwakenLayerIndex), 1)) ;
        }
    
        //Normal
        private void InitComboSettingForBaseLayer()
        {
            if (AwakenLayerIsActive())
                return;
            ResetCanTriggerNextComboForBaseLayer();
            thirdPersonController.DisableCharacterWalkingForBaseLayer();
        }
        
        private void SetCanTriggerNextComboForBaseLayer() {
            if (AwakenLayerIsActive())
                return;
            animator.SetTrigger(animID_CanTriggerNextCombo);
        }
        
        private void ResetCanTriggerNextComboForBaseLayer( ) {
            if (AwakenLayerIsActive())
                return;
            animator.ResetTrigger(animID_CanTriggerNextCombo);
        }

        //Awaken
        private void InitComboSettingForAwakenLayer()
        {
            if (!AwakenLayerIsActive())
                return;
            ResetCanTriggerNextComboForAwakenLayer();
            thirdPersonController.DisableCharacterWalkingForAwakenLayer();
        }
        
        private void SetCanTriggerNextComboForAwakenLayer() {
            if (!AwakenLayerIsActive())
                return;
            animator.SetTrigger(animID_CanTriggerNextCombo);
        }
        
        private void ResetCanTriggerNextComboForAwakenLayer( ) {
            if (!AwakenLayerIsActive())
                return;
            animator.ResetTrigger(animID_CanTriggerNextCombo);
        }
    #endregion

    #region AttackHitBox handling

        //Force 
        public void ForceDisableAttackHitBoxOfWeapon()
        {
            weaponHitBox.DisableAttackCollider();
        }
        
        public void ForceEnableAttackHitBoxOfWeapon()
        {
            weaponHitBox.EnableAttackCollider();
        }

    
        //Normal
        public void EnableNormalAttackHitBoxOfWeapon()
        {
            if (AwakenLayerIsActive())
                return;
            weaponHitBox.EnableAttackCollider();
        }
        
        public void EnableNormalAttackHitBoxOfWeaponWithPower(float skillPower)
        {
            if (AwakenLayerIsActive())
                return;
            weaponHitBox.EnableAttackColliderWithSkillPower(skillPower);
        }
        
        public void DisableNormalAttackHitBoxOfWeapon()
        {
            if (AwakenLayerIsActive())
                return;
            weaponHitBox.DisableAttackCollider();
        }
    
        
        //Awaken
        public void EnableAwakenAttackHitBoxOfWeapon()
        {
            if (!AwakenLayerIsActive())
                return;
            weaponHitBox.EnableAttackCollider();
        }
        
        public void EnableAwakenAttackHitBoxOfWeaponWithPower(float skillPower)
        {
            if (!AwakenLayerIsActive())
                return;
            weaponHitBox.EnableAttackColliderWithSkillPower(skillPower);
        }
        
        public void DisableAwakenAttackHitBoxOfWeapon()
        {
            if (!AwakenLayerIsActive())
                return;
            weaponHitBox.DisableAttackCollider();
        }

    #endregion

    #region SlashVFX
        //Normal Slashes
        private void SpawnNormalCombo1VFX()
        {
            if (AwakenLayerIsActive())
                return;                
            slashVFXManager.SpawnNormalSlashEffect(0);
        }
        private void SpawnNormalCombo2VFX()
        {
            if (AwakenLayerIsActive())
                return;                
            slashVFXManager.SpawnNormalSlashEffect(1);
        }
        private void SpawnNormalCombo3VFX()
        {
            if (AwakenLayerIsActive())
                return;                
            slashVFXManager.SpawnNormalSlashEffect(2);
        }
        private void SpawnNormalCombo4Part1VFX()
        {
            if (AwakenLayerIsActive())
                return;                
            slashVFXManager.SpawnNormalSlashEffect(3);
        }
        private void SpawnNormalCombo4Part2VFX()
        {
            if (AwakenLayerIsActive())
                return;                
            slashVFXManager.SpawnNormalSlashEffect(4);
        }
    
        //Awaken Slashes
        private void SpawnAwakenCombo1VFX()
        {
            if (AwakenLayerIsActive())
                slashVFXManager.SpawnAwakenSlashEffect(0);
        }
        
        private void SpawnAwakenCombo2VFX()
        {
            if (AwakenLayerIsActive())
                slashVFXManager.SpawnAwakenSlashEffect(1);
        }
        private void SpawnAwakenCombo3VFX()
        {
            if (AwakenLayerIsActive())
                slashVFXManager.SpawnAwakenSlashEffect(2);
        }

        private void SpawnAwakenCombo4VFXPart1()
        {
            if (AwakenLayerIsActive())
                slashVFXManager.SpawnAwakenSlashEffect(3);
        }
        
        private void SpawnAwakenCombo4VFXPart2()
        {
            if (AwakenLayerIsActive())
                slashVFXManager.SpawnAwakenSlashEffect(4);
        }
        
    #endregion
    
    #region Audio
        //Normal
        public void PlayNormalAttackAudioOfCombo1( )
        {
            if (AwakenLayerIsActive())
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(combo1Clips[Random.Range(0, combo1Clips.Length)]);
        }
        
        public void PlayNormalAttackAudioOfCombo2( )
        {
            if (AwakenLayerIsActive())
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(combo2Clips[Random.Range(0, combo2Clips.Length)]);
        }
        
        public void PlayNormalAttackAudioOfCombo3( )
        {
            if (AwakenLayerIsActive())
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(combo3Clips[Random.Range(0, combo3Clips.Length)]);
        }
        
        public void PlayNormalAttackAudioOfCombo4( )
        {
            if (AwakenLayerIsActive())
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(combo4Clips[Random.Range(0, combo4Clips.Length)]);
        }
    
        //Awaken
        public void PlayAwakenAttackAudioOfCombo1( )
        {
            if (!AwakenLayerIsActive())
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(combo1Clips[Random.Range(0, combo1Clips.Length)]);
        }
        
        public void PlayAwakenAttackAudioOfCombo2( )
        {
            if (!AwakenLayerIsActive())
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(combo2Clips[Random.Range(0, combo2Clips.Length)]);
        }
        
        public void PlayAwakenAttackAudioOfCombo3( )
        {
            if (!AwakenLayerIsActive())
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(combo3Clips[Random.Range(0, combo3Clips.Length)]);
        }
        
        public void PlayAwakenAttackAudioOfCombo4( )
        {
            if (!AwakenLayerIsActive())
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(combo4Clips[Random.Range(0, combo4Clips.Length)]);
        }
    #endregion

}
