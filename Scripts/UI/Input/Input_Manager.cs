using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Colored.Foundations;
using System.Collections.Generic;

namespace Colored.InputSystem
{
    public class Manager : Singleton<Manager>
    {
        private InputActionAsset _asset;
        private Dictionary<ActionMapType, InputActionMap> _actionMapTable;
        private Stack<InputActionMap> _contextStack;
        public DeviceType CurrentDeviceType { get; private set; }

		protected override void PostCreated()
        {
			base.PostCreated();

            _asset = Resource.Manager.Instance.Load<InputActionAsset>("Input/InputActions");
            if (_asset == null)
            {
                Logger.Error(LogCategory.System,
                    "Input Manager 초기화 실패 - InputActionAsset을 찾지 못했다. path:(Input/InputActions)");
                return;
            }
            _actionMapTable = new Dictionary<ActionMapType, InputActionMap>();
            _contextStack = new Stack<InputActionMap>();
            _registerActionMaps();
            Logger.Info(LogCategory.System, "Input.Manager initialize");

            UnityEngine.InputSystem.InputSystem.onEvent += _onInputEvent;
        }
        
        private void _registerActionMaps()
        {
            foreach (ActionMapType type in Enum.GetValues(typeof(ActionMapType)))
            {
                if (type == ActionMapType.None)
                {
                    continue;
                }
                _actionMapTable[type] = _asset.FindActionMap(type.ToString(), throwIfNotFound: true);
            }
        }

        public void PushContext(ActionMapType type)
        {
            if (_actionMapTable.TryGetValue(type, out InputActionMap map) == false)
            {
                Logger.Warning(LogCategory.System,
                    "PushContext 실패 - ActionMapTable 에서 ActionMap을 찾지 못했다. ActionMapType:({0})", type);
                return;
            }

            if (_contextStack.Count > 0)
            {
                _contextStack.Peek().Disable();   
            }
            _contextStack.Push(map);
            map.Enable();
        }

        public void PopContext()
        {
            if (_contextStack.Count == 0)
            {
                Logger.Warning(LogCategory.System,
                    "PopContext 실패 - ContextStack 이 비어있다.");
                return;
            }

            _contextStack.Pop().Disable();
            if (_contextStack.Count > 0)
            {
                _contextStack.Peek().Enable();
            }
        }

        public InputAction FindAction(ActionMapType mapType, string actionName)
        {
            if (_actionMapTable.TryGetValue(mapType, out InputActionMap map) == false)
            {
                Logger.Warning(LogCategory.System, "FindAction 실패 - ActionMapTable 에서 ActionMap을 찾지 못했다. ActionMapType:({0})", mapType);
                return null;
            }

            InputAction action = map.FindAction(actionName);
            if (action == null)
            {
                Logger.Warning(LogCategory.System, "FindAction 실패 - ActionMap 에서 Action 을 찾지 못했다. ActionName:({0})",
                    actionName);
            }
            return action;
        }
        
        private void _onInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (!eventPtr.IsA<StateEvent>())
            {
                return;
            }

            DeviceType detected;
            
            switch (device)
            {
                case UnityEngine.InputSystem.Gamepad _:
                    bool hasGamepadChange = false;
                    foreach (var control in InputControlExtensions.EnumerateChangedControls(eventPtr, device, 0.1f))
                    {
                        hasGamepadChange = true;
                        break;
                    }
                    if (!hasGamepadChange) return;
                    detected = DeviceType.Gamepad;
                    break;
                
                case UnityEngine.InputSystem.Keyboard _:
                    bool hasKeyboardChange = false;
                    foreach (var control in InputControlExtensions.EnumerateChangedControls(eventPtr, device, 0.0f))
                    {
                        hasKeyboardChange = true;
                        break;
                    }
                    if (!hasKeyboardChange) return;
                    detected = DeviceType.KeyboardMouse;
                    break;
                
                case UnityEngine.InputSystem.Mouse mouse:
                    if (!mouse.leftButton.isPressed
                        && !mouse.rightButton.isPressed
                        && !mouse.middleButton.isPressed)
                    {
                        return;
                    }
                    detected = DeviceType.KeyboardMouse;
                    break;
                default:
                    return;
            }

            if (CurrentDeviceType == detected)
            {
                return;
            }

            CurrentDeviceType = detected;
            
            if (Colored.Frameworks.Event.Dispatcher.Instance == null)
            {
                return;
            }
            Colored.Frameworks.Event.InputDeviceChanged.Dispatch(CurrentDeviceType);
            Logger.Info(LogCategory.System, "InputDeviceType changed: ({0})", CurrentDeviceType);
        }
    }
}