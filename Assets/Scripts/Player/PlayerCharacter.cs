using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;


public enum PlayerAttackMode
{
    NORMAL, AWAKEN
}

[RequireComponent(typeof(ThirdPersonController),typeof(Animator),typeof(AudioSource))]
public class PlayerCharacter : CombatCharacter
{
    [Header("Special HitBox")]
    [SerializeField] private AttackHitBox weaponHitBox;

    [Header("Audio")] 
    [SerializeField] private AudioClip[] jumpClips;
    [SerializeField] private AudioClip[] weakDamagedClips;
    [SerializeField] private AudioClip[] strongDamagedClips;
    [SerializeField] private AudioClip[] awakeClips;
    [SerializeField] private AudioClip[] combo1Clips;
    [SerializeField] private AudioClip[] combo2Clips;
    [SerializeField] private AudioClip[] combo3Clips;
    [SerializeField] private AudioClip[] combo4Clips;
    [SerializeField] private AudioClip combo1WeaponClip;
    [SerializeField] private AudioClip combo2WeaponClip;
    [SerializeField] private AudioClip combo3WeaponClip;
    [SerializeField] private AudioClip combo4Part1WeaponClip;
    [SerializeField] private AudioClip combo4Part2WeaponClip;

    
    [Header("VFX")]
    [SerializeField] private SlashVFXManager slashVFXManager;
    [SerializeField] private ParticleSystem[] awakeModeVFXs;

    [Header("Timeline")] 
    [SerializeField] private PlayableDirector characterAwakeDirector;
    
    //Audio
    private AudioSource audioSource;
    
    //Animations
    private Animator animator;
    private const int AwakenLayerIndex = 1;
    private int animID_IsDamaged;
    private int animID_CanTriggerNextCombo;
    private int animID_Awake;
    
    //TPS controller
    private ThirdPersonController thirdPersonController;

    public static PlayerCharacter Instance { get; private set; }
    public PlayerAttackMode playerAttackMode { get; private set; }
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
        animID_Awake = Animator.StringToHash("Awake");
    }
    
    public Vector3 GetPlayerWorldPosition()
    {
        return transform.position;
    }
    
    #region Audio
        public void PlayCharacterJumpAudio()
        {
            if(jumpClips==null)
                return;
                
            audioSource.Stop();
            audioSource.PlayOneShot(jumpClips[Random.Range(0, jumpClips.Length)]);
        }

        public void PlayerCharacterWeakDamagedAudio()
        {
            if(weakDamagedClips==null)
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(weakDamagedClips[Random.Range(0, weakDamagedClips.Length)]);
        }
        
        public void PlayerCharacterStrongDamagedAudio()
        {
            if(strongDamagedClips==null)
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(strongDamagedClips[Random.Range(0, strongDamagedClips.Length)]);
        }
        
        private void PlayCharacterAwakeAudio()
        {
            if(awakeClips==null)
                return;
            
            audioSource.Stop();
            audioSource.PlayOneShot(awakeClips[Random.Range(0, awakeClips.Length)]);
        }

        
        //Normal Combo
        private void PlayNormalAttackCombo1WeaponAudio()
        {  
            if (AwakenLayerIsActive())
                return;
            audioSource.PlayOneShot(combo1WeaponClip);
        }
        
        private void PlayNormalAttackCombo2WeaponAudio()
        {
            if (AwakenLayerIsActive())
                return;
            audioSource.PlayOneShot(combo2WeaponClip);
        }
        
        private void PlayNormalAttackCombo3WeaponAudio()
        {
            if (AwakenLayerIsActive())
                return;
            audioSource.PlayOneShot(combo3WeaponClip);
        }
        private void PlayNormalAttackCombo4Part1WeaponAudio()
        {
            if (AwakenLayerIsActive())
                return;
            audioSource.PlayOneShot(combo4Part1WeaponClip);
        }
        private void PlayNormalAttackCombo4Part2WeaponAudio()
        {
            if (AwakenLayerIsActive())
                return;
            audioSource.PlayOneShot(combo4Part2WeaponClip);
        }


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
    
        //Awaken Combo
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
    
    #region PlayerStateHandling
        public void SetAnimationTriggerIsDamaged()
        {
            animator.SetTrigger(animID_IsDamaged);
            //Debug.Log("Trigger is damaged");
        }

        public void SetCharacterToAwakeMode()
        {
            animator.Play(animID_Awake,0);
            animator.SetLayerWeight(AwakenLayerIndex,1);

            PlayCharacterAwakeAudio();
            foreach (ParticleSystem vfx in awakeModeVFXs) {
                vfx.Play();
            }
            
            playerAttackMode = PlayerAttackMode.AWAKEN;
            
            characterAwakeDirector.Play();
        }
        
        public void SetCharacterToNormalMode()
        {
            foreach (ParticleSystem vfx in awakeModeVFXs) {
                vfx.Stop();
            }
            playerAttackMode = PlayerAttackMode.NORMAL;
            animator.SetLayerWeight(AwakenLayerIndex,0);
        }
    

    #endregion

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
        
        private void SpawnAwakenCombo3Part1VFX()
        {
            if (AwakenLayerIsActive())
                slashVFXManager.SpawnAwakenSlashEffect(2);
        }

        private void SpawnAwakenCombo3Part2VFX()
        {
            if (AwakenLayerIsActive())
                slashVFXManager.SpawnAwakenSlashEffect(3);
        }

        private void SpawnAwakenCombo4VFXPart1()
        {
            if (AwakenLayerIsActive())
                slashVFXManager.SpawnAwakenSlashEffect(4);
        }
        
        private void SpawnAwakenCombo4VFXPart2()
        {
            if (AwakenLayerIsActive())
                slashVFXManager.SpawnAwakenSlashEffect(5);
        }
        
    #endregion
    

}
