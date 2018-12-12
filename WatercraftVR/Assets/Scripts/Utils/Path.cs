using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.Utility
{
	[System.Serializable]
	public struct Bezier
	{
		public Vector3 P0;
		public Vector3 P1;
		public Vector3 P2;

		public Vector3 P3;

		private static float delta = 0.00001f;

		public Vector3 GetGradient(float t)
		{
			var cur = GetPoint(t);
			var nex = GetPoint(t+delta);
			return ((nex - cur) / delta).normalized;
		}

		public Vector3 GetPoint(float t)
		{
			var oneMinusT = 1f - t;
			return oneMinusT * oneMinusT * oneMinusT * P0 +
				3f * oneMinusT * oneMinusT * t * P1 +
				3f * oneMinusT * t * t * P2 +
				t * t * t * P3;
		}
	}

	[System.Serializable]
	public struct Path
	{
		public Bezier[] Segments;
		// public int Current;
	}
}