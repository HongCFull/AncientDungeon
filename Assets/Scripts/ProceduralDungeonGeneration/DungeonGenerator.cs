using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;



public class DungeonGenerator : MonoBehaviour
{

    [Header("Dungeon Prefabs")]
    [SerializeField] private DungeonTile[] tilePrefabs;
    [SerializeField] private DungeonTile[] startingRoomPrefabs;
    [SerializeField] private DungeonTile[] exitRoomPrefabs;
    [SerializeField] private DungeonTile[] blockingPrefabs;
    [SerializeField] private DungeonTile[] doorPrefabs;
    
    [Header("Dungeon Generation Settings")]
    [Tooltip("The main path will have dungeonDepth + 1(root) rooms")]
    [SerializeField] [Range(0,100)] private int dungeonDepth;
    [SerializeField] [Range(0, 10)] private int branchDepth;
    [SerializeField] [Range(0, 50)] private int totalBranches;
    [SerializeField] [Range(0, 100)] private int doorPercentage;
    
    [Header("Runtime Properties")]
    [SerializeField] List<DungeonTile> generatedTiles = new List<DungeonTile>();
    [SerializeField] private List<Connector> remainingConnectors = new List<Connector>();    //Should be read only! the connectors that are available after creating the main path
    private DungeonTile tileFrom, tileTo, rootTile;
    
    
    void Start() {
        GenerateDungeon();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void GenerateDungeon() {
        GenerateDungeonMainPath();
        GetRemainingConnectors();
    }

    //Similar to DFS
    void GenerateDungeonMainPath() {
        //create starting room
        rootTile = GenerateStartingRoom();
        tileFrom = null;
        tileTo = rootTile;

        //create dungeon main path
        for (int i = 0; i < dungeonDepth; i++) {
            tileFrom = tileTo;

            if (i == dungeonDepth - 1) {
                tileTo = GenerateEndingRoom();  //create ending room
            }else {
                tileTo = GenerateTile();    //creating intermediate room for the main path
            }

            ConnectTiles();
        }
    }

    void GenerateBranches() {
        //remainingConnectors.
    }

    /// <summary>
    /// Get the remaining connectable connectors from generated tile list
    /// </summary>
    void GetRemainingConnectors() {
        if(generatedTiles.Count==0) return;

        foreach (DungeonTile tile in generatedTiles) {
            List<Connector> connectors = tile.GetAllConnectableConnectors();
            foreach (Connector connector in connectors) {
                remainingConnectors.Add(connector);
            }
        }
    }

    DungeonTile GenerateStartingRoom() {
        int index = Random.Range(0, startingRoomPrefabs.Length);
        int yRotation = Random.Range(0, 4) * 90;
        
        DungeonTile startingRoom = Instantiate(startingRoomPrefabs[index], transform.position, Quaternion.Euler(0,yRotation,0), transform);
        startingRoom.name = "Starting Room";
        startingRoom.parentTile = null;
        startingRoom.parentConnector = null;
        
        generatedTiles.Add(startingRoom);
        return startingRoom;
    }

    DungeonTile GenerateTile() {
        int index = Random.Range(0, tilePrefabs.Length);
        DungeonTile tile = Instantiate(tilePrefabs[index], transform.position, Quaternion.identity, transform);
        generatedTiles.Add(tile);

        return tile;
    }
    
    DungeonTile GenerateEndingRoom() {
        int index = Random.Range(0, startingRoomPrefabs.Length);
        DungeonTile tile = Instantiate(startingRoomPrefabs[index], transform.position, Quaternion.identity, transform);
        tile.name = "Ending Room";
        generatedTiles.Add(tile);
        
        return tile;
    }
    
    void ConnectTiles() {
        if (tileFrom == null || tileTo == null) {
            Debug.Log("one of the tiles is null");
            return;
        }
        Connector fromConnector = tileFrom.PopRandomConnectableConnector();
        if (fromConnector == null) return;
        fromConnector.isConnected = true;
        
        Connector toConnector = tileTo.PopRandomConnectableConnector();
        if (toConnector == null) return;
        toConnector.isConnected = true;
        
        //unpack the connector to outside
        Transform toConnectorTransform = toConnector.transform;
        toConnectorTransform.SetParent(this.transform);
        tileTo.transform.SetParent(toConnectorTransform);
        
        //append the toConnector to fromConnector & set the transform;
        toConnectorTransform.SetParent(fromConnector.transform);
        toConnectorTransform.localPosition = Vector3.zero;
        toConnectorTransform.localRotation = quaternion.identity;
        toConnectorTransform.Rotate(0,180,0);
    
        //restructure the tile hierarchy and make it back to a child of the generator  
        tileTo.transform.SetParent(this.transform);
        toConnectorTransform.parent = tileTo.transform;
       
        //Update tile's parent
        tileTo.parentTile = tileFrom;
        tileTo.parentConnector = fromConnector;

    }
    
    
}
