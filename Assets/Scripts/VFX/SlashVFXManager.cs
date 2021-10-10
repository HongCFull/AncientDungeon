using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashVFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] slashEffect;

    public void SpawnSlashEffect(int vfxIndex) 
    {
        ParticleSystem vfx = Instantiate<ParticleSystem>(slashEffect[vfxIndex], slashEffect[vfxIndex].gameObject.transform.position, slashEffect[vfxIndex].gameObject.transform.rotation);
        vfx.gameObject.SetActive(true);
        
        ParticleSystem.MainModule vfxMainModule = vfx.main;
        Destroy(vfx.gameObject, vfxMainModule.duration);
    }
}
