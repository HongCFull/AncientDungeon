using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DungeonEndingTile : DungeonTile
{
    private const int bossSceneIndex = 1;

    public override void OnPlayerEnterTile(Collider other) =>
        GameSceneManager.Instance.LoadSceneWithProgressBG(bossSceneIndex);

    // IEnumerator StartLoadingBossScene()
    // {
    //     AsyncOperation loadOperation = SceneManager.LoadSceneAsync(bossSceneIndex);
    //
    //     while (!loadOperation.isDone) {
    //         yield return null;
    //     }
    //     
    // }
}
