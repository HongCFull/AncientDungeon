using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakenSlash04Part2InnerSlashCollision : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("inner slash hits "+other.gameObject.name);
    }
}
