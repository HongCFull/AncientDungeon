using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashVFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] slashEffect;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vfxIndex"></param>
    /// <param name="moveWithPlayer"></param>
    /// <param name="duration"></param>
    public void SpawnSlashEffect(int vfxIndex)
    {
       // StartCoroutine()
       // Transform transformHolder = moveWithPlayer ? transform : null;

        ParticleSystem vfx = Instantiate<ParticleSystem>(slashEffect[vfxIndex], slashEffect[vfxIndex].gameObject.transform.position, slashEffect[vfxIndex].gameObject.transform.rotation);
        vfx.gameObject.SetActive(true);
        
        ParticleSystem.MainModule vfxMainModule = vfx.main;
        Destroy(vfx.gameObject, vfxMainModule.duration);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vfxIndex">The vfx index in the slashEffect that you want to instantiate</param>
    /// <param name="duration"> The duration time that the VFX will move with the player. Negative duration means the vfx is appended to player until it is destroyed. </param>
    public void SpawnSlashEffectThatFollowsPlayer(int vfxIndex,float duration)
    {
        ParticleSystem vfx = Instantiate<ParticleSystem>(slashEffect[vfxIndex], slashEffect[vfxIndex].gameObject.transform.position, slashEffect[vfxIndex].gameObject.transform.rotation,transform);
        vfx.gameObject.SetActive(true);
        
        if(duration>0f)
            StartCoroutine(UnPackVFXFromPlayerAfterSec(vfx.gameObject, duration));
        
        ParticleSystem.MainModule vfxMainModule = vfx.main;
        Destroy(vfx.gameObject, vfxMainModule.duration);
    }

    IEnumerator UnPackVFXFromPlayerAfterSec(GameObject vfxObj,float delayInSec)
    {
        yield return new WaitForSeconds(delayInSec);
        if(vfxObj==null)
            yield break;

        vfxObj.transform.parent = null;
    }
}
