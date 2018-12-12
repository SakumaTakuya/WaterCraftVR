using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.WatercraftVR.Game
{
	public class Chaser : MonoBehaviour 
	{
		[SerializeField] private Transform _target;
		[SerializeField] private Vector3 _distination;
		[SerializeField] private float _speed;
		[SerializeField] private bool _lookAt;

		private Transform _transform;

		public Vector3 Distination
		{
			get{ return _distination; }
			set{ _distination = value; }
		}

		private void Start()
		{
			_transform = transform;
		}

		private void FixedUpdate()
		{
			var dist = _target.TransformPoint(_distination);
			dist.y = _distination.y;
			_transform.position = Vector3.Slerp
			(
				_transform.position, 
				dist, 
				_speed * Time.fixedDeltaTime
			);

			if(_lookAt)
			{
				var lookPos = _target.position;
				lookPos.y = _transform.position.y;
				_transform.LookAt(lookPos);
			}

		}


	}
}

