using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakenSlash04Part2OuterSlashCollision : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("outer slash hits "+other.gameObject.name);
    }
}
