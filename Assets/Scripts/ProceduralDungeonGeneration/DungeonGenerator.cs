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

    [SerializeField] private DungeonTile[] tilePrefabs;
    [SerializeField] private DungeonTile[] startingRoomPrefabs;
        
    private DungeonTile tileFrom, tileTo;

    // Start is called before the first frame update
    void Start() {
        tileFrom = GenerateStartingRoom();
        tileTo = GenerateTile();
        ConnectTiles();

        for (int i = 0; i < 10; i++) {
            tileFrom = tileTo;
            tileTo = GenerateTile();
            ConnectTiles();
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    DungeonTile GenerateStartingRoom() {
        int index = Random.Range(0, startingRoomPrefabs.Length);
        int yRotation = Random.Range(0, 4) * 90;
        DungeonTile startingRoom = Instantiate(startingRoomPrefabs[index], transform.position, Quaternion.Euler(0,yRotation,0), transform);
        startingRoom.name = "Starting Room";
        
        return startingRoom;
    }

    DungeonTile GenerateTile() {
        int index = Random.Range(0, tilePrefabs.Length);
        DungeonTile tile = Instantiate(tilePrefabs[index], transform.position, Quaternion.identity, transform);
        return tile;
    }
    
    void ConnectTiles() {
        if (tileFrom == null || tileTo == null) {
            Debug.Log("one of the tiles is null");
            return;
        }
        Connector fromConnector = tileFrom.GetRandomConnectableConnector();
        if (fromConnector == null) return;
        fromConnector.isConnected = true;
        
        Connector toConnector = tileTo.GetRandomConnectableConnector();
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
        
    }
    
    
}
