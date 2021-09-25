using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTile : MonoBehaviour
{
    [SerializeField] private List<Connector> corridorConnector;
    [SerializeField] private List<Connector> connectedConnectors = new List<Connector>();   //should be read only
    public DungeonTile parentTile = null;
    public Connector parentConnector = null;
    
    public Connector PopRandomConnectableConnector() {
        if (corridorConnector.Count == 0) {
            Debug.Log("no connector is connectable");
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
