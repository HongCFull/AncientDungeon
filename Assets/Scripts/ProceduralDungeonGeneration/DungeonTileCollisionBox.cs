using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DungeonTileCollisionBox : MonoBehaviour
{
    [SerializeField] private DungeonTile tile;
    [SerializeField] private BoxCollider boundingBox;
    // Start is called before the first frame update
    
    public Vector3 GetGlobalCollisionBoxCenter() {

        return transform.TransformPoint(boundingBox.center);
    }

    public Vector3 GetScaledCollisionBoxHalfExtend() {

        //Vector3 scaledBoxHalfExtents = transform.TransformVector(boundingBox.size*0.5f);
        Vector3 scaledBoxHalfExtents = (boundingBox.size*0.5f);

        scaledBoxHalfExtents.x = Mathf.Abs(scaledBoxHalfExtents.x);
        scaledBoxHalfExtents.y = Mathf.Abs(scaledBoxHalfExtents.y);
        scaledBoxHalfExtents.z = Mathf.Abs(scaledBoxHalfExtents.z);

        return scaledBoxHalfExtents;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation * tile.transform.rotation;
    }
        
#if UNITY_EDITOR
    private void OnDrawGizmos() 
    {
        if(!boundingBox)
            boundingBox = GetComponent<BoxCollider>();
        
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        //BUG: Orientation problem will skew the box, need to use gizmos matrix  
        Gizmos.DrawWireCube(boundingBox.center,2*GetScaledCollisionBoxHalfExtend());
    }
    

#endif
}
