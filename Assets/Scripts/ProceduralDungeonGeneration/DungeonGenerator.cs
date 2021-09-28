using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    [Tooltip("The amount of time delay after connecting 2 tiles")]
    [SerializeField] [Range(0f, 1f)] private float generationDelay;
    
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
        StartCoroutine(GenerateDungeon());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    
    IEnumerator GenerateDungeon() {
        yield return StartCoroutine(GenerateDungeonMainPath());
        GetRemainingConnectors();
        yield return new WaitForSeconds(1);
        StartCoroutine(GenerateBranches());
    }

    //Similar to DFS
    IEnumerator GenerateDungeonMainPath() {
        GameObject mainPathObj = new GameObject();
        mainPathObj.transform.SetParent(this.transform);
        mainPathObj.transform.position = this.transform.position;
        mainPathObj.name = "Main Path";
        
        rootTile = GenerateStartingRoom();
        rootTile.transform.SetParent(mainPathObj.transform);
        
        //create dungeon main path
        yield return StartCoroutine(ExpandPathFromTile(rootTile,dungeonDepth,mainPathObj.transform));
    }

    /// <summary>
    /// Note: It doesnt generate by referring that connector but the tile owner of that connector. The remaining connector is just indicating which tile can still be expanded only!
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateBranches() {
        remainingConnectors.Shuffle();
        for (int i = 0; i < totalBranches; i++) {
            if (i >= remainingConnectors.Count) break;  //catch when remaining < totalBranches  

            DungeonTile extendFromTile = remainingConnectors[i].GetTileOwner();
            
            GameObject branchHolder = new GameObject();
            branchHolder.transform.SetParent(this.transform);
            branchHolder.transform.position = remainingConnectors[i].transform.position;
            branchHolder.name = "Branch " + (i+1);

            GameObject branchFrom = new GameObject();
            branchFrom.name = "Extended from tile: " + extendFromTile.name;
            branchFrom.transform.SetParent(branchHolder.transform);
            
            int randomBranchDepth = Random.Range(1, branchDepth + 1);
            yield return StartCoroutine(ExpandPathFromTile(extendFromTile, randomBranchDepth, branchHolder.transform));
        }
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

    DungeonTile GenerateIntermediateTile() {
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
    

    IEnumerator ExpandPathFromTile(DungeonTile tile, int depth, Transform pathHolderObject=null) {
        Debug.Log("Expanding the path from "+ tile.name);
        //If tileHolder is not specified: use default tileHolder object = the dungeonGenerator
        if (pathHolderObject == null)
            pathHolderObject = this.transform;
        
        tileTo = tile;

        for (int i = 0; i < depth; i++) {
            tileFrom = tileTo;

            if (i == depth - 1) {
                tileTo = GenerateEndingRoom();  //create ending room
                tileTo.name += " of " + pathHolderObject.name;
            }else {
                tileTo = GenerateIntermediateTile();    //creating intermediate room for the path
                tileTo.name += " "+i;   //give an index for naming clarity
            }

            ConnectTiles(pathHolderObject);
            yield return new WaitForSeconds(generationDelay);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tileHolder"></param>
    void ConnectTiles(Transform tileHolder =null) {
        if (tileFrom == null || tileTo == null) {
            Debug.Log("one of the tiles is null");
            return;
        }
        Debug.Log("From "+tileFrom.name+" to "+tileTo.name);
        
        //If tileHolder is not specified: use default tileHolder object = the dungeonGenerator
        if (tileHolder == null) 
            tileHolder = transform;

        Connector fromConnector = tileFrom.PopRandomConnectableConnector();
        if (fromConnector == null) return;
        fromConnector.isConnected = true;
        
        Connector toConnector = tileTo.PopRandomConnectableConnector();
        if (toConnector == null) return;
        toConnector.isConnected = true;
        
        //unpack the connector to outside
        Transform toConnectorTransform = toConnector.transform;
        toConnectorTransform.SetParent(tileHolder);
        tileTo.transform.SetParent(toConnectorTransform);
        
        //append the toConnector to fromConnector & set the transform;
        toConnectorTransform.SetParent(fromConnector.transform);
        toConnectorTransform.localPosition = Vector3.zero;
        toConnectorTransform.localRotation = quaternion.identity;
        toConnectorTransform.Rotate(0,180,0);
    
        //restructure the tile hierarchy and make it back to a child of the generator  
        tileTo.transform.SetParent(tileHolder);
        toConnectorTransform.SetParent(tileTo.transform);
       
        //Update tile's parent
        tileTo.parentTile = tileFrom;
        tileTo.parentConnector = fromConnector;
        

    }

    bool HasCollisionOnNewlyConnectedTiles(int attempt=0) {
        BoxCollider tileToBoundingBox = tileTo.boundingBox;
        //The displacement vector from the origin of tileTo's transform to the centroid of the box.  
        Vector3 centerOffset = tileTo.transform.right * tileToBoundingBox.center.x +
                               tileTo.transform.up * tileToBoundingBox.center.y +
                               tileTo.transform.forward * tileToBoundingBox.center.z;
        Vector3 centerOfBoundingBox = tileTo.gameObject.transform.position + centerOffset;
        
        Vector3 boxHalfExtents = tileToBoundingBox.bounds.extents;
        List<Collider> collidersHit = Physics
            .OverlapBox(centerOfBoundingBox, boxHalfExtents, Quaternion.identity, LayerMask.GetMask("Tile")).ToList();

        if (collidersHit.Count > 0) {   //if overlapped some thing
            
            //This lambda expression check if the collider collides with colliders that are not tileFrom and tileTo  
            Predicate<Collider> hasOverlappedPredicate =
            (Collider x) => {
                return (x.transform != tileFrom.gameObject.transform) &&
                       (x.transform != tileTo.gameObject.transform);
            };

            if (collidersHit.Exists(hasOverlappedPredicate)) {
                return true;
            }
            else {
                return false;
            }
        }
        return false;
    }
    

}



