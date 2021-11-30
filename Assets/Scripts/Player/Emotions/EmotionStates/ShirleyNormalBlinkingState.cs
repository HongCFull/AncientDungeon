using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirleyNormalBlinkingState : ShirleyEmotionStateBase
{
    [Header("Blinking in normal mode")]
    [SerializeField] private float blinkPeriod;
    [SerializeField] private float blinkDuration;
    public bool EnableBlink = true;
    
    [ReadOnly] private const int EyeCloseBlendShapeIndex = 35;
    private float blinkTimer;
    private bool isBlinking = false;
    
    
    public override void OnStateEnter()
    {
        
    }

    public override void TickUpdate()
    {
        if (!EnableBlink || isBlinking)
            return;
        
        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0) {
            blinkTimer = blinkPeriod;
            StartCoroutine(BlinkOnce());
        }
    }

    public override void OnStateExit()
    {
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
            skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, Mathf.Clamp(100*durationProgress / (blinkDuration / 2),0,100));
            durationProgress += Time.deltaTime;
            yield return null;
        }
        //Opening eye
        while (durationProgress >=0)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, Mathf.Clamp(100* durationProgress / (blinkDuration / 2),0,100));
            durationProgress -= Time.deltaTime;
            yield return null;
        }
        isBlinking = false;
    }

    void ResetBlendShapeValue()
    {
        skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, 0);
    }
}
