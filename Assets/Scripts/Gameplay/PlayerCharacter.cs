using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter Instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPlayerWorldPosition()
    {
        return transform.position;
    }
}
