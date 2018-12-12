using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sakkun.WatercraftVR.Game.Record;
using Sakkun.Utility;

namespace Sakkun.WatercraftVR.Game
{
	public class GhostEmitter : MonoBehaviour 
	{
		[SerializeField] private Timer _timer;
		[SerializeField] private ScoreManager _scoreManager;
		[SerializeField] private GhostInput _ghost;
		[SerializeField] private Vector3 _startPosition;
		[SerializeField] private Quaternion _startRotation;


		private void Start()
		{
			_timer.StartEvent += Emit;
		}

		private void Emit()
		{
			var datas = _scoreManager.Data;
			float len = datas.Count();
			foreach(var dataSet in datas.Indexed())
			{
				var ghost = Instantiate(
					_ghost,
					_startPosition,
					_startRotation
				);

				ghost.SetColor(
					Color.HSVToRGB(dataSet.Index() / len,0.5f,1f)
				);

				ghost.Play(dataSet.Item());
			}
		}
	}
}

