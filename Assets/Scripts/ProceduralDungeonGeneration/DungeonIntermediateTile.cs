using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonIntermediateTile : DungeonTile
{
    [Header("Enemies")] 
    [SerializeField] private bool canSpawnEnemy;
    [SerializeField] private AICharacter[] aiCharacters;
    [SerializeField] private int numOfEnemies;
    [SerializeField] private int randomOffset;
    [SerializeField] private List<Transform> spawnPositions;
    private bool hasSpawnedEnemies = false;

    public override void OnPlayerEnterTile(Collider other) 
    {
        if (canSpawnEnemy && !hasSpawnedEnemies && other.gameObject.tag.Equals("Player")) {
            hasSpawnedEnemies = true;
            SpawnAIEnemies();
        }
    }

    private void SpawnAIEnemies()
    {
        if (aiCharacters.Length <= 0)
            return;
        
        if(numOfEnemies>spawnPositions.Count)
            Debug.Log("numOfEnemies greater than spawnPositions");
        
        spawnPositions.Shuffle();

        //Get the number of spawnable enemies 
        int randomNumOfEnemies = Random.Range(Mathf.Max(0,numOfEnemies - randomOffset), numOfEnemies + randomOffset);
        int numOfEnemiesToSpawn = Mathf.Min(randomNumOfEnemies, spawnPositions.Count);

        for (int i = 0; i < numOfEnemiesToSpawn; i++) {
            Vector3 position = spawnPositions[i].position;
            AICharacter aiCharacter = Instantiate(aiCharacters[Random.Range(0, aiCharacters.Length)], position, quaternion.identity);
            aiCharacter.GetAIController().SetControllerAIToHatred(true);
        }
        
    }
}
