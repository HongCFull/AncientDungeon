using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Prefabs")]
    [SerializeField] private DungeonTile[] tilePrefabs;
    [SerializeField] private DungeonTile[] startingRoomPrefabs;
    [SerializeField] private DungeonTile[] endingRoomPrefabs;
    [SerializeField] private GameObject[] blockingPrefabs;
    [SerializeField] private GameObject[] doorPrefabs;

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
    [SerializeField] private List<DungeonTile> connectableTilesForBranching = new List<DungeonTile>();    //Should be read only! the tiles that are available after creating the main path
    private List<GameObject> pathHolderList = new List<GameObject>();
    private DungeonTile tileFrom, tileTo, rootTile, endTile;
    

    void Start() 
    {
        StartCoroutine(GenerateDungeon());
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    IEnumerator GenerateDungeon() 
    {
        yield return StartCoroutine(GenerateDungeonMainPath());
        ForceSpawningEndRoomInMainPath();
        GetConnectableTilesForBranching();
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GenerateBranches());
        yield return StartCoroutine(FillUpHolds());
        GenerateNavMeshSurface();
    }

    //Similar to DFS
    IEnumerator GenerateDungeonMainPath()
    {
        GameObject mainPathObj = GeneratePathHolderObject("Main Path", transform.position);
        rootTile = GenerateStartingRoom();
        rootTile.transform.SetParent(mainPathObj.transform);
        
        //create dungeon main path
        yield return StartCoroutine(ExpandPathFromTile(rootTile,dungeonDepth,mainPathObj.transform));
    }

    /// <summary>
    /// Note: 
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateBranches() 
    {
        connectableTilesForBranching.Shuffle();
        for (int i = 0; i < totalBranches; i++) {
            if (i >= connectableTilesForBranching.Count) break;  //catch when remaining < totalBranches  

            DungeonTile tileForExtension = connectableTilesForBranching[i];
            GameObject branchHolder = GeneratePathHolderObject("Branch " + (i+1),  connectableTilesForBranching[i].transform.position);
           
            int depth = useRandomDepthForBranches? Random.Range(1, branchDepth + 1) : branchDepth ;
            yield return StartCoroutine(ExpandPathFromTile(tileForExtension, depth, branchHolder.transform));
        }
    }

    IEnumerator FillUpHolds()
    {
        foreach (DungeonTile tile in generatedTiles) {
            if (tile.TileIsFullyConnected()) 
                continue;

            List<Connector> connectors = tile.GetAllConnectableConnectors();
            
            foreach (Connector connector in connectors) {
                connector.BlockConnector();
                //GameObject holdFiller = Instantiate(blockingPrefabs[0], connector.transform.position,connector.transform.rotation, tile.pathHolder.transform);
                //holdFiller.transform.Rotate(0f,90f,0f);
                yield return null;
            }
        }
    }

    void GenerateNavMeshSurface()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    
    
    /// <summary>
    /// Get the remaining connectable tiles (with duplicates) from generated tile list
    /// </summary>
    void GetConnectableTilesForBranching() 
    {
        if(generatedTiles.Count==0) return;

        foreach (DungeonTile tile in generatedTiles) {
            List<Connector> connectors = tile.GetAllConnectableConnectors();
            foreach (Connector connector in connectors) {
                connectableTilesForBranching.Add(connector.GetTileOwner());
            }
        }
    }

    GameObject GeneratePathHolderObject(string name, Vector3 position)
    {
        GameObject mainPathObj = new GameObject();
        pathHolderList.Add(mainPathObj);
        mainPathObj.transform.SetParent(this.transform);
        mainPathObj.transform.position = position;
        mainPathObj.name = name;
        
        return mainPathObj;
    }

    DungeonTile GenerateStartingRoom() 
    {
        int index = Random.Range(0, startingRoomPrefabs.Length);
        int yRotation = Random.Range(0, 4) * 90;
        
        DungeonTile startingRoom = Instantiate(startingRoomPrefabs[index], transform.position, Quaternion.Euler(0,yRotation,0), transform);
        startingRoom.name = "Starting Room";
        startingRoom.parentTile = null;
        startingRoom.parentConnector = null;
        
        generatedTiles.Add(startingRoom);
        return startingRoom;
    }

    DungeonTile GenerateIntermediateTile() 
    {
        int index = Random.Range(0, tilePrefabs.Length);
        DungeonTile tile = Instantiate(tilePrefabs[index], transform.position, Quaternion.identity, transform);
        generatedTiles.Add(tile);

        return tile;
    }
    
    DungeonTile GenerateEndingRoom() 
    {
        int index = Random.Range(0, startingRoomPrefabs.Length);
        DungeonTile tile = Instantiate(endingRoomPrefabs[index], transform.position, Quaternion.identity, transform);
        tile.name = "Ending Room";
        generatedTiles.Add(tile);
        
        return tile;
    }
    

    IEnumerator ExpandPathFromTile(DungeonTile tile, int depth, Transform pathObject=null) 
    {
        //Debug.Log("Expanding the path from "+ tile.name);
        
        //If tileHolder is not specified: use default tileHolder object = the dungeonGenerator
        if (pathObject == null)
            pathObject = this.transform;
        
        tileTo = tile;

        for (int i = 0; i < depth; i++) {
            tileFrom = tileTo;

            if (i == depth - 1) {
                tileTo = GenerateEndingRoom();  //create ending room
                tileTo.name += " of " + pathObject.name;
                ConnectTiles(false,pathObject);

            }else {
                tileTo = GenerateIntermediateTile();    //creating intermediate room for the path
                tileTo.name += " "+i+" of " + pathObject.name;   //give an index for naming clarity
                ConnectTiles(true,pathObject);
            }

            yield return new WaitForSeconds(generationDelay);
        }
    }
    
    /// <summary>
    /// Connect tileFrom and tileTo recursively. It handles collision when connecting tiles.  
    /// </summary>
    /// <param name="isIntermediate"> Is tileTo an intermediate room or an ending room? </param>
    /// <param name="pathObject"> The object which holds the tiles in that path </param>
    /// <param name="attempt"> How many times tried to connect this set of tileFrom and tileTo </param>
    bool ConnectTiles(bool isIntermediate,Transform pathObject =null, int attempt =0) 
    {
        if (tileFrom == null || tileTo == null) {
            //Debug.Log("one of the tiles is null");
            return false;
        }
        
        //If tileHolder is not specified: use default tileHolder object = the dungeonGenerator
        if (pathObject == null) 
            pathObject = transform;

        if (!isIntermediate && pathObject.name.Contains("Main Path")) //if is the ending room of the main path, cache it in the endTile
            endTile = tileTo;
                
        if (attempt > retryMaxCount) {
            tileFrom.RestorePreviousPoppedConnector();
            CullOutTileTo();
            tileTo = tileFrom;  //move the original tileFrom to tileTo for next depth of generation
            return false;
        }
        
        GlueTileToAndTileFromInto(pathObject);

        if (HaveCollisionOnNewlyConnectedTiles()) {
        
            tileFrom.RestorePreviousPoppedConnector();
            CullOutTileTo();
            
            if (isIntermediate) {
                tileTo= GenerateIntermediateTile();
                tileTo.name = "retry : intermediate tile";
            }else {
                tileTo= GenerateEndingRoom();
                tileTo.name = "retry : ending room";
            }
            return ConnectTiles(isIntermediate,pathObject,++attempt);
        
        }

        return true;
    }

    void GlueTileToAndTileFromInto(Transform pathHolder) 
    {
        
        Connector fromConnector = tileFrom.PopRandomConnectableConnector(); 
        if(fromConnector==null) return; // cant get any connectable connector
        
        Connector toConnector = tileTo.PopRandomConnectableConnector();
        if(toConnector==null) return; // cant get any connectable connector
        
        //unpack the connector to outside
        Transform toConnectorTransform = toConnector.transform;
        toConnectorTransform.SetParent(pathHolder);
        tileTo.transform.SetParent(toConnectorTransform);

        //append the toConnector to fromConnector & set the transform;
        toConnectorTransform.SetParent(fromConnector.transform);
        toConnectorTransform.localPosition = Vector3.zero;
        toConnectorTransform.localRotation = quaternion.identity;
        toConnectorTransform.Rotate(0, 180, 0);

        //restructure the tile hierarchy and make it back to a child of the generator  
        tileTo.transform.SetParent(pathHolder);
        toConnectorTransform.SetParent(tileTo.transform);

        //Update tile's parent
        tileTo.parentTile = tileFrom;
        tileTo.parentConnector = fromConnector;
        
        //Update tile's pathHolder
        tileFrom.pathHolder = pathHolder.gameObject;
        tileTo.pathHolder = pathHolder.gameObject;
    }


    bool HaveCollisionOnNewlyConnectedTiles() 
    {
        List<Collider> collidersHit = Physics
            .OverlapBox(tileTo.GetGlobalCollisionBoxCenter(), tileTo.GetScaledCollisionBoxHalfExtend(), tileTo.GetRotationOfCollisionBox(), LayerMask.GetMask("Tile")).ToList();
        tileTo.collidesHit = collidersHit;
        if (collidersHit.Count > 0) {   //if overlapped some thing
            
            Predicate<Collider> hasOverlappedPredicate = (Collider x) => 
            {
                return (x.transform != tileFrom.GetCollisionBoxTransform()) &&
                       (x.transform != tileTo.GetCollisionBoxTransform()) &&
                       //It MUST be the "Dungeon Tile Collision Box" which is a Box Collider
                       x.GetType()==typeof(BoxCollider);    
            };
            
            if (collidersHit.Exists(hasOverlappedPredicate)) {
               // Debug.Log(tileTo.name+" has collision");
                return true;
            }
            else {
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// Completely remove the TileTo and it's artifact 
    /// </summary>
    void CullOutTileTo() 
    {
        connectableTilesForBranching.Remove(tileTo);
        generatedTiles.Remove(tileTo);
        DestroyImmediate(tileTo.gameObject);
        tileTo = null;
    }

    /// <summary>
    /// Completely remove the last generated tile and it's artifact
    /// </summary>
    void CullOutLastGeneratedTile() 
    {
        DungeonTile lastTile = generatedTiles.Last();
        connectableTilesForBranching.Remove(lastTile);
        generatedTiles.Remove(lastTile);
        DestroyImmediate(lastTile.gameObject);
    }

    /// <summary>
    /// Force the generation of the end room in the main path if it is not spawned yet
    /// </summary>
    void ForceSpawningEndRoomInMainPath() 
    {
        while (!endTile) {
            //ending room is not spawned
            DungeonTile lastTile = generatedTiles.Last();
            
            tileFrom = lastTile.parentTile;
            tileFrom.RestorePreviousPoppedConnector();
           // Debug.Log("Replacing "+tileFrom.name+" with MainPath's ending room");
            CullOutLastGeneratedTile();
            
            tileTo = GenerateEndingRoom();
            
            if (ConnectTiles(false, pathHolderList.First().transform)) {
                endTile = tileTo;
            }
        }
    }
}



