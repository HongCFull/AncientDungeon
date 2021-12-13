using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{

    [Tooltip("The index should match with the scene index")]
    [SerializeField] private SceneBGMHolder[] sceneBGMHolders;
    private int currentSceneIndex;
    
    void Start()
    {
        sceneBGMHolders[currentSceneIndex].PlayNormalBGM(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            sceneBGMHolders[currentSceneIndex].PlayBattleBGM(true);
        
        if(Input.GetKeyDown(KeyCode.O))
            sceneBGMHolders[currentSceneIndex].PlayNormalBGM(true);

    }
}
