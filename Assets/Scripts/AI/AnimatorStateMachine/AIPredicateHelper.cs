using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIPredicateHelper 
{

    public static bool HasReachedPosition(Vector3 position, Vector3 targetPosition, float tolerance)
    {
        float distError = (targetPosition - position).magnitude;
        return distError <= tolerance;
    }
    
    public static bool HasReachedPosition(Transform position, Transform targetPosition, float tolerance)
    {
        float distError = (targetPosition.position - position.position).magnitude;
        return distError <= tolerance;
    }
}
