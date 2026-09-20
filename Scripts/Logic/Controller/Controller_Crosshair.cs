using UnityEngine;
using UnityEngine.InputSystem;

namespace Colored.Logic.Controller {

    public class Crosshair : Frameworks.Controller.Base
    {
        Vector2 _aimStickInput = Vector2.zero;
        Vector2 _aimWorldPosition = Vector2.zero;
        Vector2 _aimDirection = Vector2.zero;
        GameObject _crosshairGO;
        const float CROSSHAIR_DISTANCE = 3.0f;

        public Vector2 AimWorldPosition => _aimWorldPosition;
        public Vector2 AimDirection => _aimDirection;

        public override void PostCreated()
        {
            base.PostCreated();

            _crosshairGO = new GameObject( "[Crosshair]" );
            var renderer = _crosshairGO.AddComponent< SpriteRenderer >();
            var sprite = UnityEngine.Resources.Load< Sprite >( "Crosshair/Crosshair_03" );
            renderer.sprite = sprite;
            renderer.sortingLayerName = "Characters";
            renderer.sortingOrder = 101;
            _crosshairGO.transform.localScale = Vector3.one * 0.1f; //임시 크기
        }

        public override void PreDestory()
        {
            base.PreDestory();

            if( _crosshairGO != null ) 
            {
                UnityEngine.Object.Destroy( _crosshairGO );
                _crosshairGO = null;
            }
        }

        public override void Update( float elapsedTime )
        {
            base.Update( elapsedTime );

            if( CharacterRef.IsAlive() == false ) 
            {
                return;
            }

            Vector2 characterPosition = (Vector2)CharacterRef.Get().GetPosition();
            var deviceType = Colored.InputSystem.Manager.Instance.CurrentDeviceType;

            if( deviceType == Colored.InputSystem.DeviceType.Gamepad ) 
            {
                if( _aimStickInput.sqrMagnitude > 0.01f ) 
                {
                    _aimDirection = _aimStickInput.normalized;
                    float distance = _aimStickInput.magnitude * CROSSHAIR_DISTANCE;
                    _aimWorldPosition = characterPosition + _aimDirection * distance;
                }
            }
            else {
                Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
                var camera = Frameworks.Camera.Anchor.Instance.GetCamera();
                Vector2 mouseWorldPosition = camera.ScreenToWorldPoint( mouseScreenPosition );
                _aimWorldPosition = mouseWorldPosition;
                Vector2 diff = _aimWorldPosition - characterPosition;
                
                if( diff.sqrMagnitude > 0.001f ) 
                {
                    _aimDirection = diff.normalized;
                }
            }

            if( _crosshairGO != null ) 
            {
                _crosshairGO.transform.position = (Vector3)_aimWorldPosition;
            }
        }

        public void SetAimStickInput( Vector2 input )
        {
            _aimStickInput = input;
        }
    }
    
}
