using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colored.UI {

	public sealed class TweenAlpha : MonoBehaviour
	{
		[ SerializeField ]
		float start = 0.0f;
		[ SerializeField ]
		float end = 0.0f;
		[ SerializeField ]
		AnimationCurve curve = AnimationCurve.Linear( 0.0f, 0.0f, 1.0f, 1.0f );
		[ SerializeField ]
		bool IsLoop = false;
		[ SerializeField ]
		bool IsPingPong = false;
		[ SerializeField ]
		UnityEngine.Events.UnityEvent unityEvent = null;

		Graphic _graphic = null;
		Color _Color;
		float _t = 0.0f;
		float _lastTime = 0.0f;
		bool _called = false;

		private void Awake()
		{
			_graphic = GetComponent< Graphic >();
			if( _graphic != null ) {
				_Color = _graphic.color;
			}

			var key = curve.keys[ curve.length - 1 ];
			_lastTime = key.time;
		}

		private void Update()
		{
			if( _graphic == null ) {
				return;
			}

			_t += Time.unscaledDeltaTime;

			float evalTime = _t;

			if( IsLoop && _lastTime > 0.0f ) {
				if( IsPingPong ) {
					evalTime = Mathf.PingPong( _t, _lastTime );
				}
				else {
					evalTime = _t % _lastTime;
				}
			}
			else if( _t > _lastTime ) {
				evalTime = _lastTime;
			}

			float t = curve.Evaluate( evalTime );

			_Color.a = Mathf.Lerp( start, end, t );
			_graphic.color = _Color;

			if( unityEvent != null && _called == false ) {
				if( _t >= _lastTime ) {
					unityEvent.Invoke();
					_called = true;
				}
			}
		}

		private void OnEnable()
		{
			_t = 0.0f;
			_called = false;
		}
	}

}
