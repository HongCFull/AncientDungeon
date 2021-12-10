using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class DungeonTile : MonoBehaviour
{
    [Header("Hit Box")]
    [SerializeField] private DungeonTileCollisionBox boundingBox;
    
    [Header("Connectors")]
    [SerializeField] private List<Connector> corridorConnector;
    [SerializeField] private List<Connector> connectedConnectors = new List<Connector>();   //should be read only

    [Header("Enemies")] 
    [SerializeField] private bool canSpawnEnemy;
    [SerializeField] private AICharacter[] aiCharacters;
    [SerializeField] private int numOfEnemies;
    [SerializeField] private int randomOffset;
    [SerializeField] private List<Transform> spawnPositions;
    
    [Space]
    [Header("Debug")]
    [HideInInspector] public Connector parentConnector = null;
    [ReadOnly] public DungeonTile parentTile = null;
    [ReadOnly] public GameObject pathHolder;
    [ReadOnly] public List<Collider> collidesHit;

    //public BoxCollider boundingBox { get; private set; }
    private Connector latestPopedConnector;
    private Vector3 scaleVector;
    private bool hasSpawnedEnemies = false;
    
    private void Awake() {
        //cache boundingBox
        //boundingBox = GetComponent<BoxCollider>();
        scaleVector = transform.localScale;
    }
    
    public void SpawnEnemyOnTrigger(Collider other)
    {
        if (canSpawnEnemy && !hasSpawnedEnemies && other.gameObject.tag.Equals("Player")) {
            hasSpawnedEnemies = true;
            SpawnAIEnemies();
        }
    }
    
    /// <summary>
    /// Pop and assume the connector is connected 
    /// </summary>
    /// <returns> A random connectable connector </returns>
    public Connector PopRandomConnectableConnector() {
        if (corridorConnector.Count == 0) {
            Debug.Log("no connector is connectable in this tile");
            return null;
        }
        
        int randIndexInList = Random.Range(0, corridorConnector.Count);

        Connector selectedConnector = corridorConnector[randIndexInList];

        selectedConnector.isConnected = true;
        latestPopedConnector = selectedConnector;
        
        corridorConnector.Remove(selectedConnector);
        connectedConnectors.Add(selectedConnector);
        
        return selectedConnector;
    }

    /// <summary>
    /// Restore back the connector that is popped previously (only the latest one possible).    
    /// </summary>
    public void RestorePreviousPoppedConnector() {
        if (latestPopedConnector == null) return;
        
        corridorConnector.Add(latestPopedConnector);
        connectedConnectors.Remove(latestPopedConnector);

        latestPopedConnector.isConnected = false;

        latestPopedConnector = null;
    }

    public List<Connector> GetAllConnectableConnectors() {
        return corridorConnector;
    }

    public bool TileIsFullyConnected() {
        return connectedConnectors.Count == 0;
    }

    public Vector3 GetGlobalCollisionBoxCenter()
    {
        return boundingBox.GetGlobalCollisionBoxCenter();
    }

    public Vector3 GetScaledCollisionBoxHalfExtend() {
        return boundingBox.GetScaledCollisionBoxHalfExtend();
    }

    public Quaternion GetRotationOfCollisionBox()
    {
        return boundingBox.GetRotation();
    }

    public Transform GetCollisionBoxTransform()
    {
        return boundingBox.transform;
    }
    
    private void SpawnAIEnemies()
    {
        if (aiCharacters.Length <= 0)
            return;
        
        if(numOfEnemies>spawnPositions.Count)
            Debug.Log("numOfEnemies greater than spawnPositions");
        
        spawnPositions.Shuffle();

        int randomNumOfEnemies = Random.Range(Mathf.Max(0,numOfEnemies - randomOffset), numOfEnemies + randomOffset);
        int numOfEnemiesToSpawn = Mathf.Min(randomNumOfEnemies, spawnPositions.Count);
        for (int i = 0; i < numOfEnemiesToSpawn; i++) {
            //Vector3 position = transform.TransformPoint(spawnPositions[i].position);
            Vector3 position = spawnPositions[i].position;
            AICharacter aiCharacter = Instantiate(aiCharacters[Random.Range(0, aiCharacters.Length)], position, quaternion.identity);
            aiCharacter.GetAIController().SetControllerAIToHatred(true);
        }
        
    }
    
}
