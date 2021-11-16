using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArtificialGravityState : StateMachineBehaviour
{
    [SerializeField] private bool applyArtificialGravity;
    private CharacterController characterController;
    
    public new virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        characterController = animator.GetComponent<CharacterController>();
    }

    public new virtual void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);
        if(applyArtificialGravity)
            characterController.Move(new Vector3(0, -15f, 0f) * Time.deltaTime);
    }

    public new virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
