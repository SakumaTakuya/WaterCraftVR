using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.WatercraftVR.Game.Record
{
	public class Timer : MonoBehaviour 
	{
		private float _startTime;
		private float _stopTime;

		public float Now
		{
			get{ return Time.time - _startTime; }
		}

		public bool IsWorking{ get; private set; }

		public event System.Action StartEvent;
		public event System.Action<float> StopEvent;

		public void StartTimer()
		{
			if(IsWorking) return;
			_startTime = Time.time;
			IsWorking = true;
			StartEvent();
		}

		public void StopTimer()
		{
			if(!IsWorking) return;
			_stopTime = Time.time - _startTime;
			IsWorking = false;
			StopEvent(_stopTime);
		}
	}
}

