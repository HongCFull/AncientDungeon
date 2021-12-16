using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBGMHolder : MonoBehaviour
{
    [SerializeField] private SceneBGMScriptableObject sceneBGMSetting;
    [SerializeField] private AudioSource normalBGMAudioSource;
    [SerializeField] private AudioSource battleBGMAudioSource;
    private AudioSource currentPlayingAudioSource;
    
    //Potential Bug? : PlayBGM functions are called before start 
    public void Start()
    {
        //TODO: Check the name of this game object validate with sceneBGMSetting.GetSceneIndex() 
        if (AudioClipsAreNotAssigned())
            AssignAudioClipToAudioSource();
    }

    public void TurnOffCurrentPlayingAudioSource() => currentPlayingAudioSource.Stop();
    
    public void PlayNormalBGM(bool enableTransition)
    {
        if (AudioClipsAreNotAssigned())
            AssignAudioClipToAudioSource();
        
        if (enableTransition) {
            StartCoroutine(TransitBGMFromBattleToNormal(0.5f, 1f));
        }
        else {
            battleBGMAudioSource.Stop();
            normalBGMAudioSource.volume = sceneBGMSetting.GetNormalBGMDesiredVolume();
            normalBGMAudioSource.Play();
        }
    }

    public void PlayBattleBGM(bool enableTransition)
    {
        if (AudioClipsAreNotAssigned())
            AssignAudioClipToAudioSource();
        
        if (enableTransition) {
            StartCoroutine(TransitBGMFromNormalToBattle(0.3f,0.4f));
        }
        else {
            normalBGMAudioSource.Stop();
            battleBGMAudioSource.volume = sceneBGMSetting.GetBattleBGMDesiredVolume();
            battleBGMAudioSource.Play();
        }
    }

    IEnumerator TransitBGMFromBattleToNormal(float battleBGMVolDropDuration,float normalBGMVolGainDuration)
    {
        yield return StartCoroutine(GraduallyTurnOffAudioSource(battleBGMAudioSource, battleBGMVolDropDuration));
        StartCoroutine(GraduallyTurnOnAudioSource(normalBGMAudioSource, sceneBGMSetting.GetNormalBGMDesiredVolume(),
            normalBGMVolGainDuration));
    }
    
    IEnumerator TransitBGMFromNormalToBattle(float normalBGMVolDropDuration,float battleBGMVolGainDuration)
    {
        yield return StartCoroutine(GraduallyTurnOffAudioSource(normalBGMAudioSource, normalBGMVolDropDuration));
        StartCoroutine(GraduallyTurnOnAudioSource(battleBGMAudioSource, sceneBGMSetting.GetBattleBGMDesiredVolume(),
            battleBGMVolGainDuration));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="audioSource">The audio source to turn off</param>
    /// <param name="duration">Change of volume per second</param>
    IEnumerator GraduallyTurnOffAudioSource(AudioSource audioSource, float duration)
    {
        if(!audioSource.isPlaying)
            yield break;

        float progress = 0f;
        float volDiff = audioSource.volume;
        
        while (progress<duration) {
            audioSource.volume -= volDiff/duration * Time.deltaTime;
            progress += Time.deltaTime;
            yield return null;
        }
        audioSource.Stop();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="audioSource">The audio source to turn on</param>
    /// <param name="duration">Change of volume per second</param>
    IEnumerator GraduallyTurnOnAudioSource(AudioSource audioSource, float volume,float duration)
    {
        currentPlayingAudioSource = audioSource;
        audioSource.Play();
        float progress = 0f;
        float volDiff = volume-audioSource.volume;
        
        if(volDiff<=0)  //already turned on 
            yield break;
        
        while (progress<duration) {
            audioSource.volume += volDiff/duration * Time.deltaTime;
            progress += Time.deltaTime;
            yield return null;
        }
    }

    private bool AudioClipsAreNotAssigned() => battleBGMAudioSource.clip == null && normalBGMAudioSource.clip == null;

    private void AssignAudioClipToAudioSource()
    {
        normalBGMAudioSource.clip = sceneBGMSetting.GetNormalBGMAudioClip();
        battleBGMAudioSource.clip = sceneBGMSetting.GetBattleBGMAudioClip();
    }
}
