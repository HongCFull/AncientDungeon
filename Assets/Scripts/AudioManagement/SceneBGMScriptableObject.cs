using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSceneBGMSetting",menuName = "SceneBGMSetting")]
public class SceneBGMScriptableObject : ScriptableObject
{
    [SerializeField] private int sceneIndex;
 
    [Header("Normal BGM Settings")]
    [SerializeField] private AudioClip normalBGM;
    [SerializeField] [Range(0, 1f)] private float desiredNormalBGMVolume; 
    
    [Header("Battle BGM Settings")]
    [SerializeField] private AudioClip battleBGM;
    [SerializeField] [Range(0, 1f)] private float desiredBattleBGMVolume;
    
    public int GetSceneIndex() => sceneIndex;
    public AudioClip GetNormalBGMAudioClip() => normalBGM;
    public float GetNormalBGMDesiredVolume() => desiredNormalBGMVolume;
    public AudioClip GetBattleBGMAudioClip() => battleBGM;
    public float GetBattleBGMDesiredVolume() => desiredBattleBGMVolume;

    
}
