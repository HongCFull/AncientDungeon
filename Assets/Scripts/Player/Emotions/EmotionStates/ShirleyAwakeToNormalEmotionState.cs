using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirleyAwakeToNormalEmotionState :ShirleyEmotionStateBase
{
    [Header("Awake to Normal emotion")]
    [SerializeField] private ShirleyEmotionController emotionController;

    [SerializeField] private float eyeClosingDuration;
    [SerializeField] private float eyeKeepClosingDuration;
    [SerializeField] private float eyeOpeningDuration;

    [ReadOnly] private const int EyeCloseBlendShapeIndex = 35;
    [SerializeField] [Range(0,100)]private float EyeCloseBlendShapeValueInBlinking;
    
    [ReadOnly] private const int ALLAngryBlendShapeIndex = 49;
    private float originalALLAngryBlendShapeValue;

    private IEnumerator progress;

    protected override void Start()
    {
        base.Start();
        progress = PlayAwakeToNormalEmotionOnce();
    }

    public override void OnStateEnter()
    {
        emotionController.SetNormalBlinkingToDefaultState();
        originalALLAngryBlendShapeValue = skinnedMeshRenderer.GetBlendShapeWeight(ALLAngryBlendShapeIndex);
        
        progress = PlayAwakeToNormalEmotionOnce();
        StartCoroutine(progress);
    }

    public override void TickUpdate()
    {
    }

    public override void OnStateExit()
    {
        StopCoroutine(progress);
    }
    
    IEnumerator PlayAwakeToNormalEmotionOnce()
    {
        float eyesCloseProgress = 0;
        while (eyesCloseProgress <= eyeClosingDuration)
        {
            float currentALLAngryValue = skinnedMeshRenderer.GetBlendShapeWeight(ALLAngryBlendShapeIndex);
            //Angry -> Less angry
            skinnedMeshRenderer.SetBlendShapeWeight(ALLAngryBlendShapeIndex, Mathf.Clamp(currentALLAngryValue-originalALLAngryBlendShapeValue/eyeOpeningDuration,0,100));
            
            //Closing eyes 
            skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, Mathf.Clamp(EyeCloseBlendShapeValueInBlinking*eyesCloseProgress/eyeClosingDuration,0,100));
            eyesCloseProgress += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(eyeKeepClosingDuration);
        float eyesOpenProgress = 0;
        while (eyesOpenProgress <= eyeOpeningDuration)
        {
            float currentEyeCloseValue = skinnedMeshRenderer.GetBlendShapeWeight(EyeCloseBlendShapeIndex);
            skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, Mathf.Clamp(currentEyeCloseValue - EyeCloseBlendShapeValueInBlinking/eyeOpeningDuration,0,100));
            eyesCloseProgress += Time.deltaTime;
            yield return null;
        }

        emotionController.TriggerCurrentStateExit();
    }
}
