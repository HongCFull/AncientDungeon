using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class DungeonTile : MonoBehaviour
{
    [Header("Hit Box")]
    [SerializeField] private DungeonTileCollisionBox boundingBox;
    
    [Header("Connectors")]
    [SerializeField] private List<Connector> corridorConnector;
    private List<Connector> connectedConnectors = new List<Connector>();   //Serialize it to make it convenient for debugging
    
    [Space]
    [Header("Debug")]
    [HideInInspector] public Connector parentConnector = null;
    [HideInInspector] public  List<Collider> collidesHit;
    [ReadOnly] public DungeonTile parentTile = null;
    [ReadOnly] public GameObject pathHolder;

    private Connector latestPopedConnector;
    private Vector3 scaleVector;
    
    //Getters
    public List<Connector> GetAllConnectableConnectors() => corridorConnector;
    public bool TileIsFullyConnected() => connectedConnectors.Count == 0;
    public Vector3 GetGlobalCollisionBoxCenter() => boundingBox.GetGlobalCollisionBoxCenter();
    public Vector3 GetScaledCollisionBoxHalfExtend() => boundingBox.GetScaledCollisionBoxHalfExtend();
    public Quaternion GetRotationOfCollisionBox() => boundingBox.GetRotation();
    public Transform GetCollisionBoxTransform() => boundingBox.transform;
    
    protected virtual void Awake() {
        scaleVector = transform.localScale;
    }

    public abstract void OnPlayerEnterTile(Collider other);
    
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


   
    
}
