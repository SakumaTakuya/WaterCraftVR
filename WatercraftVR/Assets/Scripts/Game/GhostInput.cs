using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sakkun.WatercraftVR.Game.Record;
using Sakkun.WatercraftVR.Game.WaterBike;
using Sakkun.Utility;

namespace Sakkun.WatercraftVR.Game
{
	public class GhostInput : MonoBehaviour 
	{
		[SerializeField] private GhostEngine _engine;
		[SerializeField] private MeshRenderer _renderer;

		private TimeAttackData _data;
		private Transform _transform;

		private void Awake()
		{
			_transform = transform;
		}

		public void SetColor(Color color)
		{
			_renderer.material.color = color;
		}

		public void Play(TimeAttackData data)
		{
			if(data == null) 
			{
				Destroy(gameObject);
				return;
			}

			_data = data;
			StartCoroutine(PlayBack());
		}

		private IEnumerator PlayBack()
		{
			// var queue = _data.Records;
			var interval = _data.Interval;

			var cur = new InputRecord(_transform.position, _transform.rotation);

			foreach(var nex in _data.GetNext())
			{
				yield return _engine.MoveTo(cur, nex, interval);

				cur = nex;
			}
		}
	}
}

