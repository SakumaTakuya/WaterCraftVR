using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sakkun.WatercraftVR.Game.Record;

namespace Sakkun.WatercraftVR.Game.WaterBike
{
	[RequireComponent(typeof(Rigidbody))]
	public class GhostEngine : MonoBehaviour 
	{
		private Rigidbody _rigidbody;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		public IEnumerator MoveTo(InputRecord cur, InputRecord nex, float interval)
		{
			var time = 0f;
			while(time < interval)
			{
				
				var curPos = _rigidbody.position;
				var pos = Vector3.Lerp(
					cur.Position, 
					nex.Position, 
					time/interval
				);
				var rot = Quaternion.Lerp(
					cur.Rotation, 
					nex.Rotation, 
					time/interval
				);

				_rigidbody.MoveRotation(rot);
				_rigidbody.velocity = (pos - curPos) / Time.fixedDeltaTime;
				time += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			_rigidbody.velocity = Vector3.zero;
		}
	}
}

