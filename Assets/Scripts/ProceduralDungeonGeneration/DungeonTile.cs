using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTile : MonoBehaviour
{
    [SerializeField] private List<Connector> corridorConnector;
    [ReadOnly] private List<Connector> connectedConnectors = new List<Connector>();
    
    public Connector GetRandomConnectableConnector() {
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

    public bool TileIsFullyConnected() {
        return connectedConnectors.Count == 0;
    }
}
