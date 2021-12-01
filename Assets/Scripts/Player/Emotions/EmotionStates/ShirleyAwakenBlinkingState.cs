using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirleyAwakenBlinkingState : ShirleyEmotionStateBase
{
    [Header("Blinking in awaken mode")]
    [SerializeField] private float blinkPeriod;
    [SerializeField] private float blinkDuration;
    public bool EnableBlink = true;
    
  //  [ReadOnly] private const int EyeAngryBlendShapeIndex = 34;
   // [SerializeField] [Range(0,100)]private float EyeAngryBlendShapeValue;
    [ReadOnly] private const int EyeCloseBlendShapeIndex = 35;
    [SerializeField] [Range(0,100)]private float EyeCloseBlendShapeValue;
    
    [ReadOnly] private const int ALLAngryBlendShapeIndex = 49;
    [SerializeField] [Range(0,100)]private float ALLAngryBlendShapeValue;

    private float blinkTimer;
    private bool isBlinking = false;

    private IEnumerator progress;

    protected override void Start()
    {
        base.Start();
        progress = BlinkOnce();
    }

    public override void OnStateEnter()
    {
        progress = BlinkOnce();
        skinnedMeshRenderer.SetBlendShapeWeight(ALLAngryBlendShapeIndex, ALLAngryBlendShapeValue);
    }

    public override void TickUpdate()
    {
        if (!EnableBlink || isBlinking)
            return;
        
        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0) {
            blinkTimer = blinkPeriod;
            StartCoroutine(progress);
        }
    }

    public override void OnStateExit()
    {
        StopCoroutine(progress);
        ResetBlendShapeValue();
    }
    
    IEnumerator BlinkOnce()
    {
        //Debug.Log("Start Blink");
        isBlinking = true;
        float durationProgress = 0;
            
        //Closing eye
        while (durationProgress <= blinkDuration/2)
        {
            //skinnedMeshRenderer.SetBlendShapeWeight(EyeAngryBlendShapeIndex, Mathf.Clamp(EyeAngryBlendShapeValue*durationProgress / (blinkDuration / 2),0,100));
            skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, Mathf.Clamp(EyeCloseBlendShapeValue*durationProgress / (blinkDuration / 2),0,100));
            durationProgress += Time.deltaTime;
            yield return null;
        }
        //Opening eye
        while (durationProgress >=0)
        {
            //skinnedMeshRenderer.SetBlendShapeWeight(EyeAngryBlendShapeIndex, Mathf.Clamp(EyeAngryBlendShapeValue* durationProgress / (blinkDuration / 2),0,100));
            skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, Mathf.Clamp(EyeCloseBlendShapeValue*durationProgress / (blinkDuration / 2),0,100));
            durationProgress -= Time.deltaTime;
            yield return null;
        }
        isBlinking = false;
    }

    void ResetBlendShapeValue()
    {
        skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(ALLAngryBlendShapeIndex, 0);
    }
    
}
