using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class DungeonGenerator : MonoBehaviour
{

    [Header("Dungeon Prefabs")]
    [SerializeField] private DungeonTile[] tilePrefabs;
    [SerializeField] private DungeonTile[] startingRoomPrefabs;
    [SerializeField] private DungeonTile[] exitRoomPrefabs;
    [SerializeField] private DungeonTile[] blockingPrefabs;
    [SerializeField] private DungeonTile[] doorPrefabs;

    [Header("Dungeon Generation Settings:")]
    
    [Tooltip("The amount of time delay after connecting 2 tiles")]
    [SerializeField] [Range(0f, 1f)] private float generationDelay;
    
    [Tooltip("The maximum amount of retry when there is a collision when connecting newly generated tiles ")]
    [SerializeField] [Range(1, 10)] private int retryMaxCount;
    
    [Header("Dungeon Main Path Settings")]
    [Tooltip("The main path will have dungeonDepth + 1(root) rooms")]
    [SerializeField] [Range(0,100)] private int dungeonDepth;
    
    [Header("Branching Settings")]
    [SerializeField] [Range(0, 50)] private int totalBranches;
    [SerializeField] [Range(0, 10)] private int branchDepth;
    [SerializeField] private bool useRandomDepthForBranches;

    [Header("Decorations")]
    [SerializeField] [Range(0, 100)] private int doorPercentage;
    
    
    [Header("Runtime Properties")]
    [SerializeField] List<DungeonTile> generatedTiles = new List<DungeonTile>();
    [SerializeField] private List<Connector> connectorsForBranching = new List<Connector>();    //Should be read only! the connectors that are available after creating the main path
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
        GetConnectorsForBranching();
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
        connectorsForBranching.Shuffle();
        for (int i = 0; i < totalBranches; i++) {
            if (i >= connectorsForBranching.Count) break;  //catch when remaining < totalBranches  

            DungeonTile tileForExtension = connectorsForBranching[i].GetTileOwner();
            
            GameObject branchHolder = new GameObject();
            branchHolder.transform.SetParent(this.transform);
            branchHolder.transform.position = connectorsForBranching[i].transform.position;
            branchHolder.name = "Branch " + (i+1);

            GameObject branchFrom = new GameObject();
            branchFrom.name = "Extended from tile: " + tileForExtension.name;
            branchFrom.transform.SetParent(branchHolder.transform);
            
            
            int randomBranchDepth = useRandomDepthForBranches? Random.Range(1, branchDepth + 1) : branchDepth ;
            yield return StartCoroutine(ExpandPathFromTile(tileForExtension, randomBranchDepth, branchHolder.transform));
        }
    }

    /// <summary>
    /// Get the remaining connectable connectors from generated tile list
    /// </summary>
    void GetConnectorsForBranching() {
        if(generatedTiles.Count==0) return;

        foreach (DungeonTile tile in generatedTiles) {
            List<Connector> connectors = tile.GetAllConnectableConnectors();
            foreach (Connector connector in connectors) {
                connectorsForBranching.Add(connector);
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
        //Debug.Log("Expanding the path from "+ tile.name);
        
        //If tileHolder is not specified: use default tileHolder object = the dungeonGenerator
        if (pathHolderObject == null)
            pathHolderObject = this.transform;
        
        tileTo = tile;

        for (int i = 0; i < depth; i++) {
            tileFrom = tileTo;

            if (i == depth - 1) {
                tileTo = GenerateEndingRoom();  //create ending room
                tileTo.name += " of " + pathHolderObject.name;
                ConnectTiles(false,pathHolderObject);

            }else {
                tileTo = GenerateIntermediateTile();    //creating intermediate room for the path
                tileTo.name += " "+i+" of " + pathHolderObject.name;   //give an index for naming clarity
                ConnectTiles(true,pathHolderObject);
            }

            yield return new WaitForSeconds(generationDelay);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tileHolder"></param>
    void ConnectTiles(bool isIntermediate,Transform tileHolder =null, int attempt =0) {
        if (tileFrom == null || tileTo == null) {
            Debug.Log("one of the tiles is null");
            return;
        }

        if (attempt > retryMaxCount) {
            CullOutTileTo();
            tileTo = tileFrom;  //move the original tileFrom to tileTo for next depth generation
            return;
        }
        
        //If tileHolder is not specified: use default tileHolder object = the dungeonGenerator
        if (tileHolder == null) 
            tileHolder = transform;

        Connector fromConnector = tileFrom.PopRandomConnectableConnector(); 
        if(fromConnector==null) return; // cant get any connectable connector
        
        Connector toConnector = tileTo.PopRandomConnectableConnector();
        if(toConnector==null) return; // cant get any connectable connector

        TileToTransformation(tileHolder, toConnector, fromConnector);

        if (HasCollisionOnNewlyConnectedTiles()) {
 
            tileFrom.RestorePreviousPoppedConnector();  
            
            CullOutTileTo();
            if (isIntermediate) {
                tileTo= GenerateIntermediateTile();
                tileTo.name = "retry : intermediate tile";
            }else {
                tileTo= GenerateEndingRoom();
                tileTo.name = "retry : ending room";
            }
            ConnectTiles(isIntermediate,tileHolder,++attempt);

        }
            
    }

    void TileToTransformation(Transform tileHolder, Connector toConnector, Connector fromConnector) {
        
        //unpack the connector to outside
        Transform toConnectorTransform = toConnector.transform;
        toConnectorTransform.SetParent(tileHolder);
        tileTo.transform.SetParent(toConnectorTransform);

        //append the toConnector to fromConnector & set the transform;
        toConnectorTransform.SetParent(fromConnector.transform);
        toConnectorTransform.localPosition = Vector3.zero;
        toConnectorTransform.localRotation = quaternion.identity;
        toConnectorTransform.Rotate(0, 180, 0);

        //restructure the tile hierarchy and make it back to a child of the generator  
        tileTo.transform.SetParent(tileHolder);
        toConnectorTransform.SetParent(tileTo.transform);

        //Update tile's parent
        tileTo.parentTile = tileFrom;
        tileTo.parentConnector = fromConnector;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool HasCollisionOnNewlyConnectedTiles() {
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

    void CullOutTileTo() {
        //remove last element
        //generatedTiles.RemoveAt(generatedTiles.Count - 1);
        generatedTiles.Remove(tileTo);
        DestroyImmediate(tileTo.gameObject);
        tileTo = null;
    }
    
}



