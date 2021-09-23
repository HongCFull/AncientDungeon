using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorConnector : MonoBehaviour
{
    [SerializeField] private Vector2 corridorSize;

    private void OnDrawGizmos() {
        
        //Draw forward vector at the centroid of the corridor
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        
        //Draw Boarders
        Vector3 leftUp = transform.position - transform.right * corridorSize.x / 2 + transform.up * corridorSize.y / 2;
        Vector3 leftDown = transform.position - transform.right * corridorSize.x / 2 - transform.up * corridorSize.y / 2;

        Vector3 rightUp = transform.position + transform.right * corridorSize.x / 2 + transform.up * corridorSize.y / 2;
        Vector3 rightDown = transform.position + transform.right * corridorSize.x / 2 - transform.up * corridorSize.y / 2;
        
        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(leftUp, rightUp);
        Gizmos.DrawLine(leftDown, rightDown);
        Gizmos.DrawLine(leftUp, leftDown);
        Gizmos.DrawLine(rightUp, rightDown);
        
        //Draw Diagonals
        Gizmos.DrawLine(leftUp, rightDown);
        Gizmos.DrawLine(rightUp, leftDown);


    }
}
