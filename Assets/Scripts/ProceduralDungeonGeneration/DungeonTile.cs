using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider),typeof(NavMeshSurface))]
public class DungeonTile : MonoBehaviour
{
    [Tooltip("Connectors")]
    [SerializeField] private List<Connector> corridorConnector;
    [SerializeField] private List<Connector> connectedConnectors = new List<Connector>();   //should be read only

    [Tooltip("Enemies")] 
    [SerializeField] private AICharacter[] aiCharacters;
    
    [HideInInspector] public Connector parentConnector = null;
    [ReadOnly] public DungeonTile parentTile = null;
    [ReadOnly] public GameObject pathHolder;
    [ReadOnly] public List<Collider> collidesHit;
    
    public BoxCollider boundingBox { get; private set; }
    private NavMeshSurface navMeshSurface;
    private Connector latestPopedConnector;
    private Vector3 scaleVector;
    
    
    private void Awake() {
        //cache boundingBox
        boundingBox = GetComponent<BoxCollider>();
        navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
        scaleVector = transform.localScale;

        SpawnAIEnemies();
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

        return transform.TransformPoint(boundingBox.center);
    }

    public Vector3 GetScaledCollisionBoxHalfExtend() {

        Vector3 scaledBoxHalfExtents = transform.TransformVector(boundingBox.size*0.5f);
        scaledBoxHalfExtents.x = Mathf.Abs(scaledBoxHalfExtents.x);
        scaledBoxHalfExtents.y = Mathf.Abs(scaledBoxHalfExtents.y);
        scaledBoxHalfExtents.z = Mathf.Abs(scaledBoxHalfExtents.z);

        return scaledBoxHalfExtents;
    }

    private void SpawnAIEnemies()
    {
        if (aiCharacters.Length <= 0)
            return;
        Vector3 worldPos = transform.TransformPoint(Vector3.zero);
        Instantiate(aiCharacters[Random.Range(0, aiCharacters.Length)], worldPos, quaternion.identity);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos() 
    {
        if(!boundingBox)
            boundingBox = GetComponent<BoxCollider>();

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(GetGlobalCollisionBoxCenter(),2*GetScaledCollisionBoxHalfExtend());
    }

#endif
}
