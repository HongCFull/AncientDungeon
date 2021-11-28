using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class EmotionController : MonoBehaviour
{
    [Header("Blinking")]
    [SerializeField] private float blinkPeriod;
    [SerializeField] private float blinkDuration;
    public bool EnableBlink = true;
    [ReadOnly] private const int EyeCloseBlendShapeIndex = 35;
    
    [Space]
    [Header("Damaged Face")]
    [SerializeField][Range(0,2f)] private float damagedFaceDuration;
    [SerializeField] [Range(0,100)] private float EyeCloseValue;
    [ReadOnly] private const int MTHAngryBlendShapeIndex = 21;
    [SerializeField] [Range(0,100)] private float MTHAngryValue;
    [ReadOnly] private const int BRWSorrowBlendShapeIndex = 47;
    [SerializeField] [Range(0,100)] private float BRWSorrowValue;
    [ReadOnly] private const int MTHOBlendShapeIndex = 53;
    [SerializeField] [Range(0,100)] private float MTHOValue;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private float blinkTimer;
    private bool isBlinking = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!skinnedMeshRenderer)
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        blinkTimer = blinkPeriod;
    }

    // Update is called once per frame
    void Update()
    {
        if (!EnableBlink || isBlinking)
            return;
        
        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0) {
            blinkTimer = blinkPeriod;
            StartCoroutine(BlinkOnce());
        }
        
    }

    #region Blinking
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

    #endregion

    #region DamagedFace
    
        public void ActDamagedFaceOnceWrapper()
        {
            StartCoroutine(ActDamagedFaceOnce());
        }

        IEnumerator ActDamagedFaceOnce()
        {
            Debug.Log("Start acting damaged face");
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
        }

    #endregion
}
