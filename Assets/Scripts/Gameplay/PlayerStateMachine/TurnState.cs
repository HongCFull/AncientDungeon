using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BUG: EnterLocoMotionState which is the parent StateMachine will disable RM option
public class TurnState : StateMachineBehaviour
{
  private bool originalRMOption;
  private int animIDIsTurning;
  private int animIDAngle;
  private int animIDCachedTurningAngle;
  
  private float cachedTurningAngle;
  
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    base.OnStateEnter(animator, stateInfo, layerIndex);
    InitializeVariables(animator);
    animator.applyRootMotion=true;
    animator.SetBool(animIDIsTurning,true);
    animator.SetFloat(animIDCachedTurningAngle,cachedTurningAngle);

  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    base.OnStateExit(animator, stateInfo, layerIndex);
    animator.applyRootMotion=originalRMOption;
    animator.SetBool(animIDIsTurning,false);
    animator.SetFloat(animIDCachedTurningAngle,0);

  }

  void InitializeVariables(Animator animator)
  {
    animIDIsTurning = Animator.StringToHash("IsTurning");
    animIDAngle = Animator.StringToHash("Angle");
    animIDCachedTurningAngle = Animator.StringToHash("CachedTurningAngle");

    originalRMOption = animator.applyRootMotion;
    cachedTurningAngle = animator.GetFloat(animIDAngle);
  }
}
