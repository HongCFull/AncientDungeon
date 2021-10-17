using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AIVision : MonoBehaviour
{
    public bool canSeePlayer { get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
            canSeePlayer = true;
        Debug.Log("Enter ai vision");
    }

    private void OnTriggerStay(Collider other)
    {
//        Debug.Log("stay inside");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
            canSeePlayer = false;
        Debug.Log("exit ai vision");

    }
}
