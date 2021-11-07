using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.WSA;

/// <summary>
/// The GameObject attached with this script should in the bottom mid of the connector.
/// </summary>
public class Connector : MonoBehaviour
{
    [SerializeField] private GameObject blockingPrefab;
    [SerializeField] private DungeonTile tileOwner;
    [SerializeField] private Vector2 corridorSize;
    [ReadOnly] public bool isConnected = false;
    
    public DungeonTile GetTileOwner() {
        return tileOwner;
    }
    
    private void OnDrawGizmos() {
        
        //Draw forward vector at the centroid of the corridor
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + transform.up*corridorSize.y/2, transform.position + transform.up*corridorSize.y/2 + transform.forward);
        
        //Draw Boarders
//        Vector3 leftUp = transform.position - transform.right * corridorSize.x / 2 + transform.up * corridorSize.y / 2;
//        Vector3 leftDown = transform.position - transform.right * corridorSize.x / 2 - transform.up * corridorSize.y / 2;

//        Vector3 rightUp = transform.position + transform.right * corridorSize.x / 2 + transform.up * corridorSize.y / 2;
//        Vector3 rightDown = transform.position + transform.right * corridorSize.x / 2 - transform.up * corridorSize.y / 2;
        Vector3 leftUp = transform.position - transform.right * corridorSize.x / 2 + transform.up * corridorSize.y;
        Vector3 leftDown = transform.position - transform.right * corridorSize.x / 2 ;

        Vector3 rightUp = transform.position + transform.right * corridorSize.x / 2 + transform.up * corridorSize.y ;
        Vector3 rightDown = transform.position + transform.right * corridorSize.x / 2 ;

        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(leftUp, rightUp);
        Gizmos.DrawLine(leftDown, rightDown);
        Gizmos.DrawLine(leftUp, leftDown);
        Gizmos.DrawLine(rightUp, rightDown);
        
        //Draw Diagonals
        Gizmos.DrawLine(leftUp, rightDown);
        Gizmos.DrawLine(rightUp, leftDown);

    }

    public void BlockConnector()
    {
        GameObject blockObj =
            Instantiate(blockingPrefab,  gameObject.transform);
        blockObj.transform.localScale=Vector3.one;
       // blockObj.transform.Rotate(0f,90f,0f);
    }
}
