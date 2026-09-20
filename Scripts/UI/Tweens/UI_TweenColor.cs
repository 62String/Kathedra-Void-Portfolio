using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colored.UI {

	public sealed class TweenColor : MonoBehaviour
	{
		[ SerializeField ]
		Color color = Color.white;
		[ SerializeField ]
		AnimationCurve curve = AnimationCurve.Linear( 0.0f, 0.0f, 1.0f, 1.0f );
		[ SerializeField ]
		UnityEngine.Events.UnityEvent unityEvent = null;

		Graphic _graphic = null;
		Color _startColor;
		float _t = 0.0f;
		float _lastTime = 0.0f;
		bool _called = false;

		private void Awake()
		{
			_graphic = GetComponent< Graphic >();
			if( _graphic != null ) {
				_startColor = _graphic.color;
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
			float t = curve.Evaluate( _t );

			_graphic.color = Color.Lerp( _startColor, color, t );

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
