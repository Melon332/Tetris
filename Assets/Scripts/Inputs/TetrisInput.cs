//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/Inputs/TetrisInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Inputs
{
    public partial class @TetrisInput : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @TetrisInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""TetrisInput"",
    ""maps"": [
        {
            ""name"": ""TetrisMap"",
            ""id"": ""9533b022-09e4-445a-a3b9-e3659d06fb1b"",
            ""actions"": [
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""f5721982-8684-49f9-ad3c-ac33e90440f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""6f0038e8-de05-4031-889f-5de6661906f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""550d7001-f1bc-4bd3-9a50-d8a2d4c8049d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cd58eae4-8768-42c4-94b1-8adefaf1efa7"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""752633f2-bb03-4efb-9833-2ec90f5edeb5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0df796f7-416d-4c9c-a9ff-827be721f61e"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb98fe6e-6488-47be-9e7f-0d700683a81f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d950517-da1e-432a-861c-42ea78c36a59"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee5f3339-93d8-45c2-9e3c-c70c46f4275a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // TetrisMap
            m_TetrisMap = asset.FindActionMap("TetrisMap", throwIfNotFound: true);
            m_TetrisMap_Left = m_TetrisMap.FindAction("Left", throwIfNotFound: true);
            m_TetrisMap_Right = m_TetrisMap.FindAction("Right", throwIfNotFound: true);
            m_TetrisMap_Down = m_TetrisMap.FindAction("Down", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // TetrisMap
        private readonly InputActionMap m_TetrisMap;
        private ITetrisMapActions m_TetrisMapActionsCallbackInterface;
        private readonly InputAction m_TetrisMap_Left;
        private readonly InputAction m_TetrisMap_Right;
        private readonly InputAction m_TetrisMap_Down;
        public struct TetrisMapActions
        {
            private @TetrisInput m_Wrapper;
            public TetrisMapActions(@TetrisInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Left => m_Wrapper.m_TetrisMap_Left;
            public InputAction @Right => m_Wrapper.m_TetrisMap_Right;
            public InputAction @Down => m_Wrapper.m_TetrisMap_Down;
            public InputActionMap Get() { return m_Wrapper.m_TetrisMap; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(TetrisMapActions set) { return set.Get(); }
            public void SetCallbacks(ITetrisMapActions instance)
            {
                if (m_Wrapper.m_TetrisMapActionsCallbackInterface != null)
                {
                    @Left.started -= m_Wrapper.m_TetrisMapActionsCallbackInterface.OnLeft;
                    @Left.performed -= m_Wrapper.m_TetrisMapActionsCallbackInterface.OnLeft;
                    @Left.canceled -= m_Wrapper.m_TetrisMapActionsCallbackInterface.OnLeft;
                    @Right.started -= m_Wrapper.m_TetrisMapActionsCallbackInterface.OnRight;
                    @Right.performed -= m_Wrapper.m_TetrisMapActionsCallbackInterface.OnRight;
                    @Right.canceled -= m_Wrapper.m_TetrisMapActionsCallbackInterface.OnRight;
                    @Down.started -= m_Wrapper.m_TetrisMapActionsCallbackInterface.OnDown;
                    @Down.performed -= m_Wrapper.m_TetrisMapActionsCallbackInterface.OnDown;
                    @Down.canceled -= m_Wrapper.m_TetrisMapActionsCallbackInterface.OnDown;
                }
                m_Wrapper.m_TetrisMapActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Left.started += instance.OnLeft;
                    @Left.performed += instance.OnLeft;
                    @Left.canceled += instance.OnLeft;
                    @Right.started += instance.OnRight;
                    @Right.performed += instance.OnRight;
                    @Right.canceled += instance.OnRight;
                    @Down.started += instance.OnDown;
                    @Down.performed += instance.OnDown;
                    @Down.canceled += instance.OnDown;
                }
            }
        }
        public TetrisMapActions @TetrisMap => new TetrisMapActions(this);
        public interface ITetrisMapActions
        {
            void OnLeft(InputAction.CallbackContext context);
            void OnRight(InputAction.CallbackContext context);
            void OnDown(InputAction.CallbackContext context);
        }
    }
}
