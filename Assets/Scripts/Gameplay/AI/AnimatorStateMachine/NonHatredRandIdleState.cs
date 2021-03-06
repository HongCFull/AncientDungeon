using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class NonHatredRandIdleState : StateMachineBehaviour
{
    [Header("Random Wandering Setting")]
    [SerializeField] [Range(0, 10)] private float randWanderingPeriod;
    [SerializeField] [Range(0, 5)] private float randWanderingPeriodOffset;

    [Header("Random Idle Animation Setting")] 
    
    [Tooltip("How many idle animation clips are there in this state")]
    [SerializeField] [Range(0, 20)] private int totalIdleAnimClips;
    
    private int idleIndexAnimID;
    private int nonHatredRandWanderingAnimID;
    
    private float randWanderingDelay;
    private float timer;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CacheReferences(animator);
        SetRandomIdleIndexAnimID(animator);
        GetRandWanderingDelay();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        timer += Time.deltaTime;
        if (timer >= randWanderingDelay) {
            timer = 0f;
            animator.SetBool(nonHatredRandWanderingAnimID,true);
        }
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(nonHatredRandWanderingAnimID,false);
    }

    void CacheReferences(Animator animator)
    {
        idleIndexAnimID = Animator.StringToHash("idleIndex");
        nonHatredRandWanderingAnimID = Animator.StringToHash("nonHatredRandWandering");
    }

    void GetRandWanderingDelay()
    {
        randWanderingDelay = randWanderingPeriod + Random.Range(0, randWanderingPeriodOffset);
    }

    void SetRandomIdleIndexAnimID(Animator animator)
    {
        animator.SetInteger(idleIndexAnimID,Random.Range(0,totalIdleAnimClips));
    }
    
}
