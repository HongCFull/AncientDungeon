using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{

    [Tooltip("The index should match with the scene index")]
    [SerializeField] private SceneBGMHolder[] sceneBGMHolders;
    public static BGMManager Instance { get; private set; }
    private int currentSceneIndex;

    void Start()
    {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
            return;
        }

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneBGMHolders[currentSceneIndex].PlayNormalBGM(true);
        SceneManager.sceneLoaded+=PlayNormalBGMOfThisSceneWrapper;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded-=PlayNormalBGMOfThisSceneWrapper;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            sceneBGMHolders[currentSceneIndex].PlayBattleBGM(true);
        
        if(Input.GetKeyDown(KeyCode.O))
            sceneBGMHolders[currentSceneIndex].PlayNormalBGM(true);

    }

    void PlayNormalBGMOfThisSceneWrapper(Scene scene, LoadSceneMode mode)
    {
        sceneBGMHolders[currentSceneIndex].TurnOffCurrentPlayingAudioSource();
        PlayNormalBGMOfScene(scene.buildIndex);
        currentSceneIndex = scene.buildIndex;
    }
    
    public void PlayNormalBGMOfThisScene( )
    {
        Debug.Log("BGM of "+currentSceneIndex);
        sceneBGMHolders[currentSceneIndex].PlayNormalBGM(true);
    }

    public void PlayBattleBGMOfThisScene( )
    {
        sceneBGMHolders[currentSceneIndex].PlayBattleBGM(true);
    }

    private void PlayNormalBGMOfScene(int sceneIndex)
    {
        sceneBGMHolders[sceneIndex].PlayNormalBGM(true);
    }
}
