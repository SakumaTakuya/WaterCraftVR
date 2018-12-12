using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Sakkun.Utility;

namespace Sakkun.WatercraftVR.Game.Record
{
	public class Recorder : MonoBehaviour 
	{
		[SerializeField, Range(0.01f, 5f)] private float _recordInterval = 1f;
		[SerializeField] private Transform _target;
		[SerializeField] private Timer _timer;
		[SerializeField] private ScoreManager _scoreManager;

		private TimeAttackData _data;
		private bool _isRecording;

		private void Awake()
		{
			_timer.StartEvent += StartRecord;
			_timer.StopEvent  += EaseInStop;
		}

		public void StartRecord()
		{
			_isRecording = true;
			StartCoroutine(RecordInput());
		}

		public void StopRecord(float time)
		{
			_isRecording = false;
			SaveRecordAsync(time);
		}

		// 同期を待つ必要がないためvoid
		public void SaveRecordAsync(float time) 
		{
			if(_data == null) return;
			_data.FinishedTime = time;
			// await PlayerPrefsUtility.SaveAsync<TimeAttackData>("test", _data);
			_scoreManager.SaveRanking(_data);
			_scoreManager.SaveCurrent(_data);
		}

		private IEnumerator RecordInput()
		{
			_data = new TimeAttackData(_recordInterval);
			while(_isRecording)
			{
				_data.AddKey(_target.position, _target.rotation);
				yield return new WaitForSeconds(_recordInterval);
			}
		}

		private void EaseInStop(float time)
		{
			StartCoroutine(EaseInStopEnumrator(time));
		}

		private IEnumerator EaseInStopEnumrator(float time)
		{
			var t = 0f;
			while(t < _recordInterval * 2f)
			{
				t += Time.deltaTime;
				yield return null;
			}
            StopRecord(time);
		}
	}
}

