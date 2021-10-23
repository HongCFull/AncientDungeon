using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    [SerializeField] private SlashVFXManager slashVFXManager;
    public static PlayerCharacter Instance { get; private set; }

    public SlashVFXManager GetSlashVFXManager() => slashVFXManager;
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
