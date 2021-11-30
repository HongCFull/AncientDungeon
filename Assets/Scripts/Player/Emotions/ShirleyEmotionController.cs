using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirleyEmotionController : MonoBehaviour
{
   // [SerializeField] private ShirleyEmotionStateBase defaultEmotion;
    [SerializeField] private ShirleyNormalBlinkingState normalBlinkingState;
    [SerializeField] private ShirleyDamagedEmotionState damagedEmotionState;
    [SerializeField] private ShirleyAwakenBlinkingState awakenBlinkingState;

    [HideInInspector] public bool exitCurrentStateIsTriggered = false;
        
    private ShirleyEmotionStateBase currentState;
    private ShirleyEmotionStateBase defaultState;
    
    // Start is called before the first frame update
    void Start()
    {
        defaultState = normalBlinkingState;
        currentState = normalBlinkingState;
    }

    // Update is called once per frame
    void Update()
    {
        if (exitCurrentStateIsTriggered) {
            exitCurrentStateIsTriggered = false;
            SetToDefaultState();
        }
        currentState.TickUpdate();
        
    }

    public void SetNormalBlinkingToDefaultState()
    {
        defaultState = normalBlinkingState;
    }
    
    public void SetAwakenBlinkingToDefaultState()
    {
        defaultState = awakenBlinkingState;
    }

    public void SetToDefaultState()
    {
        currentState.OnStateExit();
        currentState = defaultState;
        currentState.OnStateEnter();
    }

    public void SetToNormalBlinkingMode()
    {
        currentState.OnStateExit();
        currentState = normalBlinkingState;
        currentState.OnStateEnter();
    }
    
    public void SetToAwakenBlinkingMode()
    {
        currentState.OnStateExit();
        currentState = awakenBlinkingState;
        currentState.OnStateEnter();
    }

    public void PlayDamagedEmotionOnce()
    {
        currentState.OnStateExit();
        currentState = damagedEmotionState;
        currentState.OnStateEnter();
    }
    
    
}
