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
    private List<AICharacter> hatredAICharacters = new List<AICharacter>();
    private int currentSceneIndex;

    void OnEnable()
    {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
            return;
        }
        sceneBGMHolders[currentSceneIndex].PlayNormalBGM(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            sceneBGMHolders[currentSceneIndex].PlayBattleBGM(true);
        
        if(Input.GetKeyDown(KeyCode.O))
            sceneBGMHolders[currentSceneIndex].PlayNormalBGM(true);

    }

    //TODO: rename this function
    public void RegisterToPlayNormalBGMInThisScene(AICharacter aiCharacter)
    {
        hatredAICharacters.Remove(aiCharacter);
        
        if(hatredAICharacters.Count==0)
            sceneBGMHolders[currentSceneIndex].PlayNormalBGM(true);
    }

    //TODO: rename this function
    public void PlayBattleBGMInThisScene(AICharacter aiCharacter)
    {
        hatredAICharacters.Add(aiCharacter);
        if(hatredAICharacters.Count==1)
            sceneBGMHolders[currentSceneIndex].PlayBattleBGM(true);
    }

}
