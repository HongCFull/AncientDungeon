using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirleyDamagedEmotionState : ShirleyEmotionStateBase
{
    [Header("Damaged Emotion")] 
    [SerializeField] private ShirleyEmotionController emotionController;
    
    [SerializeField][Range(0,2f)] private float damagedFaceDuration;
    
    [ReadOnly] private const int EyeCloseBlendShapeIndex = 35;
    [SerializeField] [Range(0,100)] private float EyeCloseValue;
    
    [ReadOnly] private const int MTHAngryBlendShapeIndex = 21;
    [SerializeField] [Range(0,100)] private float MTHAngryValue;
    
    [ReadOnly] private const int BRWSorrowBlendShapeIndex = 47;
    [SerializeField] [Range(0,100)] private float BRWSorrowValue;
   
    [ReadOnly] private const int MTHOBlendShapeIndex = 53;
    [SerializeField] [Range(0,100)] private float MTHOValue;

    private IEnumerator progress;


    protected override void Start()
    {
        base.Start();
        progress = ActDamagedFaceOnce();
    }

    public override void OnStateEnter()
    {
        progress = ActDamagedFaceOnce();
        StartCoroutine(progress);
    }

    public override void TickUpdate()
    {
    }

    public override void OnStateExit()
    {
        StopCoroutine(progress);
        //ResetBlendShape();
    }
    
    IEnumerator ActDamagedFaceOnce()
    {
        float durationProgress=0f;
        while (durationProgress <= damagedFaceDuration/ 2)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, Mathf.Clamp(EyeCloseValue *durationProgress / (damagedFaceDuration / 2),0,100));
            skinnedMeshRenderer.SetBlendShapeWeight(MTHAngryBlendShapeIndex, Mathf.Clamp(MTHAngryValue *durationProgress / (damagedFaceDuration / 2),0,100));
            skinnedMeshRenderer.SetBlendShapeWeight(BRWSorrowBlendShapeIndex, Mathf.Clamp(BRWSorrowValue *durationProgress / (damagedFaceDuration / 2),0,100));
            skinnedMeshRenderer.SetBlendShapeWeight(MTHOBlendShapeIndex, Mathf.Clamp(MTHOValue *durationProgress / (damagedFaceDuration / 2),0,100));
            durationProgress += Time.deltaTime;
            yield return null;
        }
        while (durationProgress >=0 )
        {
            skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, Mathf.Clamp(EyeCloseValue *durationProgress / (damagedFaceDuration / 2),0,100));
            skinnedMeshRenderer.SetBlendShapeWeight(MTHAngryBlendShapeIndex, Mathf.Clamp(MTHAngryValue *durationProgress / (damagedFaceDuration / 2),0,100));
            skinnedMeshRenderer.SetBlendShapeWeight(BRWSorrowBlendShapeIndex, Mathf.Clamp(BRWSorrowValue *durationProgress / (damagedFaceDuration / 2),0,100));
            skinnedMeshRenderer.SetBlendShapeWeight(MTHOBlendShapeIndex, Mathf.Clamp(MTHOValue *durationProgress / (damagedFaceDuration / 2),0,100));
            durationProgress -= Time.deltaTime;
            yield return null;
        }

        emotionController.TriggerCurrentStateExit();
    }

    void ResetBlendShape()
    {
        skinnedMeshRenderer.SetBlendShapeWeight(EyeCloseBlendShapeIndex, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(MTHAngryBlendShapeIndex, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(BRWSorrowBlendShapeIndex, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(MTHOBlendShapeIndex, 0);
    }
}
