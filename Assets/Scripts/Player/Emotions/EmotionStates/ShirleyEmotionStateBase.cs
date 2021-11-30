using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public abstract class ShirleyEmotionStateBase : MonoBehaviour
{
    protected SkinnedMeshRenderer skinnedMeshRenderer;
    public abstract void OnStateEnter();
    public abstract void TickUpdate();
    public abstract void OnStateExit();

    void Start() 
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }
    
}
