using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class PlayerInput : MonoBehaviour, TetrisInput.ITetrisMapActions
    {
        private TetrisInput inputs;

        private void Start()
        {
            inputs = new TetrisInput();
            inputs.TetrisMap.SetCallbacks(this);
            inputs.Enable();
        }

        public void OnLeft(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Debug.LogWarning("I pressed left once!");
            }
        }

        public void OnRight(InputAction.CallbackContext context)
        {
            while (context.phase == InputActionPhase.Performed)
            {
                Debug.LogWarning("I pressed right once!");   
            }
        }
    }
}
