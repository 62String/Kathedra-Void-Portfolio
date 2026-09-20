using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colored.UI {

	public sealed class TweenRotate : MonoBehaviour
	{
		[ SerializeField ]
		float speed = 1.0f;

		RectTransform _rt;
		float _rot = 0.0f;

		private void Awake()
		{
			_rt = GetComponent< RectTransform >();
		}

		private void Update()
		{
			_rot += ( Time.smoothDeltaTime * speed );
			_rt.localRotation = Quaternion.Euler( 0, 0, _rot );
		}
	}

}
