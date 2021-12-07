using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class KeyCodeTriggerableUISlot : MonoBehaviour
{
   [SerializeField] protected KeyCode keyCodeToTrigger;

   [SerializeField] protected Image coolDownFilter;
   public KeyCode GetTriggerKeyCode() => keyCodeToTrigger;
   
   //update cool down timer 

   public void StartCoolDownFilterWrapper(float coolDownInSec)
   {
      if (!coolDownFilter)
         return;
      StartCoroutine(StartCoolDownFilter(coolDownInSec));
   }
   
   IEnumerator StartCoolDownFilter(float coolDownInSec)
   {
      if (Mathf.Approximately(coolDownInSec,0f)) {
         yield break;
      }
      coolDownFilter.fillAmount = 1;
      
      float progress = coolDownInSec;
      float coolDownSpeed = 1 / coolDownInSec;
      
      while (progress >= 0) {
         coolDownFilter.fillAmount -= coolDownSpeed*Time.deltaTime;
         progress -= Time.deltaTime;
         yield return null;
      }
   }

   
}
