using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashVFXManager : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private ParticleSystem[] normalSlashEffect;
    [SerializeField] private ParticleSystem[] awakenSlashEffect;
    [SerializeField] private ParticleSystem[] spiralSlashEffect; 
    [SerializeField] private ParticleSystem horizontalSlashEffect; 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vfxIndex"></param>
    /// <param name="moveWithPlayer"></param>
    /// <param name="duration"></param>
    public void SpawnNormalSlashEffect(int vfxIndex)
    {
       if (playerCharacter.AwakenLayerIsActive())
           return;
       
       ParticleSystem vfx = Instantiate<ParticleSystem>(normalSlashEffect[vfxIndex], normalSlashEffect[vfxIndex].gameObject.transform.position, normalSlashEffect[vfxIndex].gameObject.transform.rotation);
       vfx.gameObject.SetActive(true);
        
       ParticleSystem.MainModule vfxMainModule = vfx.main;
       Destroy(vfx.gameObject, vfxMainModule.duration);
    }
    
    
    public void SpawnAwakenSlashEffect(int vfxIndex)
    {
        if (!playerCharacter.AwakenLayerIsActive())
            return;

        if (awakenSlashEffect[vfxIndex] == null)
            return;
        
        ParticleSystem vfx = Instantiate<ParticleSystem>(awakenSlashEffect[vfxIndex], awakenSlashEffect[vfxIndex].gameObject.transform.position, awakenSlashEffect[vfxIndex].gameObject.transform.rotation);
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
        ParticleSystem vfx = Instantiate<ParticleSystem>(normalSlashEffect[vfxIndex], normalSlashEffect[vfxIndex].gameObject.transform.position, normalSlashEffect[vfxIndex].gameObject.transform.rotation,transform);
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

    #region SlashSkillVFX
        public void SpawnSpiralSlashEffectSlashEffect(int vfxIndex)
        {
            ParticleSystem vfx = Instantiate<ParticleSystem>(spiralSlashEffect[vfxIndex], spiralSlashEffect[vfxIndex].gameObject.transform.position, spiralSlashEffect[vfxIndex].gameObject.transform.rotation);
            vfx.gameObject.SetActive(true);
            
            ParticleSystem.MainModule vfxMainModule = vfx.main;
            Destroy(vfx.gameObject, vfxMainModule.duration);
        }
        
        public void SpawnHorizontalSlashEffectSlashEffect()
        {
            ParticleSystem vfx = Instantiate<ParticleSystem>(horizontalSlashEffect, horizontalSlashEffect.gameObject.transform.position, horizontalSlashEffect.gameObject.transform.rotation);
            vfx.gameObject.SetActive(true);
        
            ParticleSystem.MainModule vfxMainModule = vfx.main;
            Destroy(vfx.gameObject, vfxMainModule.duration);
        }


    #endregion
}
