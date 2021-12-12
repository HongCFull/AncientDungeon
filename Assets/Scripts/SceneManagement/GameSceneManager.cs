using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }
    [SerializeField] private SceneLoadingCanvas sceneLoadingCanvas;
    
    void Start()
    {
        if (Instance) {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void LoadSceneWithProgressBG(int sceneIndex) => StartCoroutine(LoadSceneAndUpdateProgressBG(sceneIndex));
    
    IEnumerator LoadSceneAndUpdateProgressBG(int sceneIndex)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!loadOperation.isDone) {
            sceneLoadingCanvas.ShowLoadingBGAndProgressBar(sceneIndex,loadOperation.progress*100f);
            yield return null;
        }
        sceneLoadingCanvas.CloseAndResetLoadingBackground();
    }
}
