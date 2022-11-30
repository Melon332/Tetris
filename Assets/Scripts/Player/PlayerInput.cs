using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Inputs;

namespace Player
{
    public class PlayerInput : MonoBehaviour, TetrisInput.ITetrisMapActions
    {
        private TetrisInput inputs;

        public Action OnLeftPressed;
        public Action OnRightPressed;

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
                OnLeftPressed?.Invoke();
                Debug.LogWarning("I pressed left once!");
            }
        }

        public void OnRight(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                OnRightPressed?.Invoke();
                Debug.LogWarning("I pressed right once!");   
            }
        }
    }
}
