using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.WatercraftVR.Game.Record
{
	[System.Serializable]
	public class TimeAttackData
	{
		public readonly List<InputRecord> Records;
		public float FinishedTime{ get; set; }
		public readonly float Interval;


		public TimeAttackData(float interval)
		{
			this.Records = new List<InputRecord>();
			this.FinishedTime = float.MaxValue;
			this.Interval = interval;
		}

		public void AddKey(Vector3 position, Quaternion rotation)
		{
			Records.Add(new InputRecord(position, rotation));
		}

		public IEnumerable<InputRecord> GetNext()
		{
			foreach(var data in Records)
			{
				yield return data;
			}
		}
	}
}
