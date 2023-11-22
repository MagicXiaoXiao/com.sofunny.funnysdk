// GENERATED AUTOMATICALLY FROM 'Packages/com.sofunny.funnysdk/Core/UIModule/Login/PC/PCLoginPageActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace SoFunny.FunnySDK.UIModule
{
    public class @PCLoginPageActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PCLoginPageActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PCLoginPageActions"",
    ""maps"": [
        {
            ""name"": ""Base"",
            ""id"": ""7f0e73ab-9087-4cb9-b724-b731e4405c0a"",
            ""actions"": [
                {
                    ""name"": ""SwitchFocus"",
                    ""type"": ""Button"",
                    ""id"": ""a2964ceb-fb16-4952-9ba4-2ab696d61d11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CommitForm"",
                    ""type"": ""Button"",
                    ""id"": ""b0cad406-c946-4dce-8b5a-bae58db449ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2120c035-1990-4e4a-89ea-67d63373051a"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchFocus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c90597c8-5f0e-4faa-bfb2-27521ff5cefe"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CommitForm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Base
            m_Base = asset.FindActionMap("Base", throwIfNotFound: true);
            m_Base_SwitchFocus = m_Base.FindAction("SwitchFocus", throwIfNotFound: true);
            m_Base_CommitForm = m_Base.FindAction("CommitForm", throwIfNotFound: true);
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

        // Base
        private readonly InputActionMap m_Base;
        private IBaseActions m_BaseActionsCallbackInterface;
        private readonly InputAction m_Base_SwitchFocus;
        private readonly InputAction m_Base_CommitForm;
        public struct BaseActions
        {
            private @PCLoginPageActions m_Wrapper;
            public BaseActions(@PCLoginPageActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @SwitchFocus => m_Wrapper.m_Base_SwitchFocus;
            public InputAction @CommitForm => m_Wrapper.m_Base_CommitForm;
            public InputActionMap Get() { return m_Wrapper.m_Base; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(BaseActions set) { return set.Get(); }
            public void SetCallbacks(IBaseActions instance)
            {
                if (m_Wrapper.m_BaseActionsCallbackInterface != null)
                {
                    @SwitchFocus.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnSwitchFocus;
                    @SwitchFocus.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnSwitchFocus;
                    @SwitchFocus.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnSwitchFocus;
                    @CommitForm.started -= m_Wrapper.m_BaseActionsCallbackInterface.OnCommitForm;
                    @CommitForm.performed -= m_Wrapper.m_BaseActionsCallbackInterface.OnCommitForm;
                    @CommitForm.canceled -= m_Wrapper.m_BaseActionsCallbackInterface.OnCommitForm;
                }
                m_Wrapper.m_BaseActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @SwitchFocus.started += instance.OnSwitchFocus;
                    @SwitchFocus.performed += instance.OnSwitchFocus;
                    @SwitchFocus.canceled += instance.OnSwitchFocus;
                    @CommitForm.started += instance.OnCommitForm;
                    @CommitForm.performed += instance.OnCommitForm;
                    @CommitForm.canceled += instance.OnCommitForm;
                }
            }
        }
        public BaseActions @Base => new BaseActions(this);
        public interface IBaseActions
        {
            void OnSwitchFocus(InputAction.CallbackContext context);
            void OnCommitForm(InputAction.CallbackContext context);
        }
    }
}
