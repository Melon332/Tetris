using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Inputs;

namespace Player
{
    public class PlayerInput : MonoBehaviour, TetrisInput.ITetrisMapActions
    {
        private TetrisInput inputs;

        public Action<int> OnLeftPressed;
        public Action<int> OnRightPressed;
        public Action OnDownPressed;

        private void Start()
        {
            inputs = new TetrisInput();
            inputs.TetrisMap.SetCallbacks(this);
            inputs.Enable();
        }

        public void OnLeft(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                OnLeftPressed?.Invoke(-1);
                Debug.LogWarning("I pressed left once!");
            }
        }

        public void OnRight(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                OnRightPressed?.Invoke(1);
                Debug.LogWarning("I pressed right once!");
            }
        }

        public void OnDown(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                OnDownPressed?.Invoke();
                Debug.LogWarning("I pressed down once!");
            }
        }
    }
}
