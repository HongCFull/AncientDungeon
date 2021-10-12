using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class DungeonTile : MonoBehaviour
{
    [SerializeField] private List<Connector> corridorConnector;
    [SerializeField] private List<Connector> connectedConnectors = new List<Connector>();   //should be read only
    
    [HideInInspector] public Connector parentConnector = null;
    [ReadOnly] public DungeonTile parentTile = null;
    [ReadOnly] public GameObject pathHolder;
    
    public BoxCollider boundingBox { get; private set; }
    private Connector latestPopedConnector;
    private Vector3 scaleVector;
    private void Awake() {
        //cache boundingBox
        boundingBox = GetComponent<BoxCollider>();
        scaleVector = transform.localScale;
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

    public Vector3 GetGlobalCollisionBoxCenter() {
        Vector3 centerOffset = transform.right * boundingBox.center.x *scaleVector.x+
                               transform.up * boundingBox.center.y *scaleVector.y+
                               transform.forward * boundingBox.center.z*scaleVector.z;
        return transform.position + centerOffset;
    }

    public Vector3 GetScaledCollisionBoxHalfExtend() {
        Vector3 originalBoxHalfExtents = boundingBox.bounds.extents;
        Vector3 scaledBoxHalfExtents = new Vector3(
            scaleVector.x * originalBoxHalfExtents.x , scaleVector.y * originalBoxHalfExtents.y, scaleVector.z * originalBoxHalfExtents.z
        );
        return scaledBoxHalfExtents;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos() 
    {
        if(!boundingBox)
            boundingBox = GetComponent<BoxCollider>();

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetGlobalCollisionBoxCenter(),GetScaledCollisionBoxHalfExtend());
    }

#endif
}
