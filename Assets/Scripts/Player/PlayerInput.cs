using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Inputs;
using TetrisBoard;

namespace Player
{
    public class PlayerInput : MonoBehaviour, TetrisInput.ITetrisMapActions
    {
        private TetrisInput inputs;

        public Action<EMoveTiles> OnLeftPressed;
        public Action<EMoveTiles> OnRightPressed;
        public Action OnDownPressed;

        private void Start()
        {
            inputs = new TetrisInput();
            inputs.TetrisMap.SetCallbacks(this);
            inputs.Enable();
        }

        public void OnLeft(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnLeftPressed?.Invoke(EMoveTiles.ELeft);
            }
        }

        public void OnRight(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnRightPressed?.Invoke(EMoveTiles.ERight);
            }
        }

        public void OnDown(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnDownPressed?.Invoke();
            }
        }
    }
}
