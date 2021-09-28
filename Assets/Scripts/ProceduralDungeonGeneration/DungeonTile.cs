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
    [HideInInspector] public DungeonTile parentTile = null;
    [HideInInspector] public Connector parentConnector = null;
    
    public BoxCollider boundingBox { get; private set; }

    private void Start() {
        //cache boundingBox
        boundingBox = GetComponent<BoxCollider>();
    }

    public Connector PopRandomConnectableConnector() {
        if (corridorConnector.Count == 0) {
            Debug.Log("no connector is connectable in this tile");
            return null;
        }
        
        int randIndexInList = Random.Range(0, corridorConnector.Count);

        Connector selectedConnector = corridorConnector[randIndexInList];
        corridorConnector.Remove(selectedConnector);
        connectedConnectors.Add(selectedConnector);
        return selectedConnector;
    }

    public List<Connector> GetAllConnectableConnectors() {
        return corridorConnector;
    }

    public bool TileIsFullyConnected() {
        return connectedConnectors.Count == 0;
    }
}
