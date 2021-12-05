using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [FormerlySerializedAs("starterAssetsInputs")] [Header("Output")]
        public PlayerInputHandler playerInputHandler;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            playerInputHandler.HandleMoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            playerInputHandler.HandleLookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            playerInputHandler.HandleJumpInput(virtualJumpState);
        }

        // public void VirtualSprintInput(bool virtualSprintState)
        // {
        //     starterAssetsInputs.SprintInput(virtualSprintState);
        // }
        //
    }

}
