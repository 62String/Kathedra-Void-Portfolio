using Colored.Foundations;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Colored.UI
{
	
	public class IBase : MonoBehaviour
	{
		protected Frameworks.Event.Holder EventHolder { get;} = new Frameworks.Event.Holder();
		
		public virtual void PostCreated()
		{
		}

		protected virtual void OnDestroy()
		{
			if (Colored.Frameworks.Event.Dispatcher.Instance == null)
			{
				return;
			}
			EventHolder.Disconnect();
		}
	}

    public class Base< _DataType > : IBase
		where _DataType : Resource.SerializedField.SF_UI_Base 
    {
		protected _DataType SerializedField { get; private set; }

        public override void PostCreated()
        {
            base.PostCreated();

			SerializedField = GetComponent< _DataType >(); 
			
			EventHolder.Connect<Colored.Frameworks.Event.InputDeviceChanged>(_onInputDeviceChanged);
			_onInputDeviceTypeChanged(InputSystem.Manager.Instance.CurrentDeviceType);
        }

        private void _onInputDeviceChanged(Colored.Frameworks.Event.InputDeviceChanged data)
        {
	        _onInputDeviceTypeChanged(data.Device);
        }

        protected virtual void _onInputDeviceTypeChanged(InputSystem.DeviceType type)
        {
	        if (UnityEngine.EventSystems.EventSystem.current == null)
	        {
		        return;
	        }
	        
	        switch (type)
	        {
		        case InputSystem.DeviceType.Gamepad:
			        GameObject defaultSelected = _getDefaultSelectedObject();
			        if (defaultSelected != null)
			        {
				        StartCoroutine(_selectNextFrame(defaultSelected));
			        }
			        break;
		        
		        case InputSystem.DeviceType.KeyboardMouse:
			        UnityEngine.EventSystems.EventSystem.current
				        .SetSelectedGameObject(null);
			        break;
	        }
        }
        private System.Collections.IEnumerator _selectNextFrame(GameObject target)
        {
	        yield return null;
	        if (Colored.InputSystem.Manager.Instance.CurrentDeviceType == Colored.InputSystem.DeviceType.Gamepad)
	        {
		        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(target);
	        }
        }

        protected virtual GameObject _getDefaultSelectedObject()
        {
	        return null;
        }
        
    }
}