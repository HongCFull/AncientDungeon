using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special case of the Skill UI Slot
/// Awake Skill can't be replaced
/// Instead of reducing the portion of the filter, this UI slot will raise the skill icon portion gradually
/// </summary>
public class ShirleyAwakeUISlot : ShirleySkillUISlot
{
    public void StartAwakeCoolDownFilterWrapper(float coolDownInSec)
    {
        StartCoroutine(StartAwakeCoolDownFilter(coolDownInSec));
    }

    IEnumerator StartAwakeCoolDownFilter(float coolDownInSec)
    {
        if (Mathf.Approximately(coolDownInSec,0f)) {
            yield break;
        }
        coolDownFilter.fillAmount = 0;
        
        float progress = 0;
        float coolDownSpeed = 1 / coolDownInSec;
        while (progress <= coolDownInSec) {
            coolDownFilter.fillAmount += coolDownSpeed*Time.deltaTime;
            progress += Time.deltaTime;
            yield return null;
        }
    }
}
