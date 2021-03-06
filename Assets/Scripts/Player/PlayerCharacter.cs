using System;
using System.Collections;
using System.Collections.Generic;
using HitBoxDefinition;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public enum PlayerAttackMode
{
    NORMAL, AWAKEN
}

/// <summary>
/// Responsible for interacting with the animator & animation callback
/// Animation event in another inactive layer is called...
/// Animation event callback function signature is bad :( 
/// </summary>
[RequireComponent(typeof(ThirdPersonController),typeof(Animator),typeof(AudioSource))]
public class PlayerCharacter : CombatCharacter
{
   // [Header("Player Stats Settings")]
  //  [SerializeField] [Range(0,Mathf.Infinity)] private float awakenDuration;
    
    [Header("Special HitBox")]
    [SerializeField] private AttackHitBox weaponHitBox;

    [Header("Battle State Change")]
    [SerializeField] private UnityEvent OnAwakeAttackMode;
    [SerializeField] private UnityEvent OnNormalAttackMode;
    
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
    [SerializeField] private AudioClip awakenCombo3Part1WeaponClip;
    [SerializeField] private AudioClip awakenCombo3Part2WeaponClip;
    [SerializeField] private AudioClip awakenCombo4Part1WeaponClip;
    [SerializeField] private AudioClip awakenCombo4Part2WeaponClip;

    [Header("VFX")]
    [SerializeField] private SlashVFXManager slashVFXManager;
    [SerializeField] private ParticleSystem[] awakeModeVFXs;

    [Header("Timeline")] 
    [SerializeField] private PlayableDirector characterAwakeDirector;
    
    //Animations
    private const int AwakenLayerIndex = 1;
    private int animID_CanTriggerNextCombo;
    private int animID_Grounded;
    private int stateID_Awake;
    
    //TPS controller
    private ThirdPersonController thirdPersonController;

    //Since some stateMachine behaviors can't ref to PlayerCharacter :(
    //need to have an Instance for this
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

        audioSource = GetComponent<AudioSource>();
        thirdPersonController = GetComponent<ThirdPersonController>();

        animID_CanTriggerNextCombo = Animator.StringToHash("canTriggerNextCombo");
        animID_Grounded = Animator.StringToHash("grounded");
        stateID_Awake = Animator.StringToHash("Awake");
    }
    public override void DisableAllAttackHitBoxes() 
    {
        weaponHitBox.DisableAttackCollider();
    }

    public override void DisableAllReceiveHitBoxes()
    {
        foreach (ReceiveHitBox receiveHitBox in receiveHitBoxes)
            receiveHitBox.DisableReceiveHitBox();
    }

    public Vector3 GetPlayerWorldPosition() => transform.position;
    
    #region GetAnimatorStateInfo
        public bool AnimatorStateCanBeInterrupted() => animator.GetBool(animID_stateCanBeInterrupted);
        public bool AnimatorIsGrounded() => animator.GetBool(animID_Grounded);

    #endregion
    
    #region PlayerStateHandling

        /// <summary>
        /// Set the animator state with the hashedStateID.
        /// </summary>
        /// <param name="hashedStateID"> The animID hash of the state name </param>
        public void SetAnimatorStateTo(int hashedStateID)
        {
            animator.Play(hashedStateID);
        }
        

        public void SetCharacterToAwakeMode()
        {
            animator.Play(stateID_Awake);
            animator.SetLayerWeight(AwakenLayerIndex,1);

            PlayCharacterAwakeAudio();
            foreach (ParticleSystem vfx in awakeModeVFXs) {
                vfx.Play();
            }
            playerAttackMode = PlayerAttackMode.AWAKEN;
            
            //characterAwakeDirector.Play();
            // Invoke(nameof(SetCharacterToNormalMode),());
            OnAwakeAttackMode.Invoke();
        }
        
        public void SetCharacterToNormalMode()
        {
            foreach (ParticleSystem vfx in awakeModeVFXs) {
                vfx.Stop();
            }
            playerAttackMode = PlayerAttackMode.NORMAL;
            animator.SetLayerWeight(AwakenLayerIndex,0);
            OnNormalAttackMode.Invoke();
        }
        

    #endregion

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
            
            //audioSource.Stop();
            audioSource.PlayOneShot(combo1Clips[Random.Range(0, combo1Clips.Length)]);
        }
        
        public void PlayNormalAttackAudioOfCombo2( )
        {
            if (AwakenLayerIsActive())
                return;
            
            // audioSource.Stop();
            audioSource.PlayOneShot(combo2Clips[Random.Range(0, combo2Clips.Length)]);
        }
        
        public void PlayNormalAttackAudioOfCombo3( )
        {
            if (AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(combo3Clips[Random.Range(0, combo3Clips.Length)]);
        }
        
        public void PlayNormalAttackAudioOfCombo4( )
        {
            if (AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(combo4Clips[Random.Range(0, combo4Clips.Length)]);
        }
    
        //Awaken Combo
        public void PlayAwakenAttackCombo1WeaponAudio()
        {
            if (!AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(combo1WeaponClip);
        }
        
        public void PlayAwakenAttackCombo2WeaponAudio()
        {
            if (!AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(combo2WeaponClip);
        }
        public void PlayAwakenAttackCombo3Part1WeaponAudio()
        {
            if (!AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(awakenCombo3Part1WeaponClip);
        }
        public void PlayAwakenAttackCombo3Part2WeaponAudio()
        {
            if (!AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(awakenCombo3Part2WeaponClip);
        }
        public void PlayAwakenAttackCombo4Part1WeaponAudio()
        {
            if (!AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(awakenCombo4Part1WeaponClip);
        }
        public void PlayAwakenAttackCombo4Part2WeaponAudio()
        {
            if (!AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(awakenCombo4Part2WeaponClip);
        }
        
        public void PlayAwakenAttackAudioOfCombo1()
        {
            if (!AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(combo1Clips[Random.Range(0, combo1Clips.Length)]);
        }
        
        public void PlayAwakenAttackAudioOfCombo2()
        {
            if (!AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(combo2Clips[Random.Range(0, combo2Clips.Length)]);
        }
        
        public void PlayAwakenAttackAudioOfCombo3()
        {
            if (!AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(combo3Clips[Random.Range(0, combo3Clips.Length)]);
        }
        
        public void PlayAwakenAttackAudioOfCombo4()
        {
            if (!AwakenLayerIsActive())
                return;
            
            //audioSource.Stop();
            audioSource.PlayOneShot(combo4Clips[Random.Range(0, combo4Clips.Length)]);
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

        public void ForceEnableAttackHitBoxOfWeaponWithPower(float skillPower)
        {
            weaponHitBox.EnableAttackColliderWithSkillPower(skillPower);
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

}
