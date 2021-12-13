using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class FallenAngelDamagedState : AIDamagedState
{
    [SerializeField] [Range(0,100f)]private float tpAfterDamagedProbability;
    private int animIDTeleport;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);
        animIDTeleport = Animator.StringToHash("teleport");

        if(ShouldTP())
            animator.SetBool(animIDTeleport,true);
        else
            base.OnStateEnter(animator, stateInfo, layerIndex);
        
    }

    private bool ShouldTP()
    {
        float weight = Random.Range(0, 100f);

        if (weight <= tpAfterDamagedProbability)
            return true;
        return false;
    }
    
    
}
