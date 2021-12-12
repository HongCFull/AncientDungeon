using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadingCanvas : MonoBehaviour
{
    [Tooltip("This progress bar is shared among all scene loading backgrounds")]
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private List<GameObject> loadSceneBackgrounds;
    
    private static SceneLoadingCanvas Instance;
    private GameObject activeLoadingBackground;
    private void Start()
    {
        if (Instance != null) {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void ShowLoadingBGAndProgressBar(int sceneIndex,float progressPercentage)
    {
        //Clear previous active loading background if forget to close
        if(activeLoadingBackground!=null && activeLoadingBackground.activeSelf)
            activeLoadingBackground.SetActive(false);

        activeLoadingBackground = loadSceneBackgrounds[sceneIndex];
        if(!activeLoadingBackground.activeSelf)
            activeLoadingBackground.SetActive(true);
        
        progressBar.gameObject.SetActive(true);
        progressBar.UpdateProgressTo(progressPercentage);
    }

    public void CloseAndResetLoadingBackground()
    {
        progressBar.CloseAndResetProgressBar();
        activeLoadingBackground.SetActive(false);
    }
}
