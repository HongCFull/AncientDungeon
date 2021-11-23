using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VFXSpawner : MonoBehaviour
{
    public static VFXSpawner Instance { get; private set; }

    private ObjectPool<ParticleSystem> vfxPool;
    private void Start()
    {
        if (!Instance)
            Instance = this;
    }

    public void SpawnOneTimeVFX(ParticleSystem particle, Vector3 position, Quaternion rotation)
    {
        ParticleSystem newParticle = Instantiate<ParticleSystem>(particle, position, rotation) ;
       // StartCoroutine()
    }
}
