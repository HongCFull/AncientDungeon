using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirleyNormalToAwakeEmotion : ShirleyEmotionStateBase
{
   [Header("Normal to Awake emotion")] 
   [SerializeField] private ShirleyEmotionController emotionController;
   
   [SerializeField] private float eyeCloseDuration;
   [SerializeField] private float eyeOpenDuration;
   
   [ReadOnly] private const int EyeCloseBlendShapeIndex = 35;
   [SerializeField] [Range(0,100)]private float EyeCloseBlendShapeInitalValue;
   
   [ReadOnly] private const int ALLAngryBlendShapeIndex = 49;
   [SerializeField] [Range(0,100)]private float ALLAngryBlendShapeValue;
   
   private IEnumerator progress;

   protected override void Start()
   {
      base.Start();
      progress = PlayNormalToAwakeEmotionOnce();
   }
   
   public override void OnStateEnter()
   {
      skinnedMeshRenderer.SetBlendShapeWeight(ALLAngryBlendShapeIndex,ALLAngryBlendShapeValue);
      skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex,EyeCloseBlendShapeInitalValue);
        
      emotionController.SetAwakenBlinkingToDefaultState();
      
      progress = PlayNormalToAwakeEmotionOnce();
      StartCoroutine(progress);
   }

   public override void TickUpdate()
   {
   }

   public override void OnStateExit()
   {
      StopCoroutine(progress);
   }

   IEnumerator PlayNormalToAwakeEmotionOnce()
   {
      yield return new WaitForSeconds(eyeCloseDuration);
      float durationProgress = 0;
      while (durationProgress<=eyeOpenDuration)
      {
         float currentBlendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(EyeCloseBlendShapeIndex);
         skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, Mathf.Clamp(currentBlendShapeWeight-EyeCloseBlendShapeInitalValue/eyeOpenDuration,0,100));
         durationProgress += Time.deltaTime;
         yield return null;
      }

      emotionController.TriggerCurrentStateExit();
   }
}
