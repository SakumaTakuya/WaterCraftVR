using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.WatercraftVR.Game.Record
{
	[System.Serializable]
	public struct InputRecord
	{
		private float _positionX;
		private float _positionY;
		private float _positionZ;
		private float _rotationX;
		private float _rotationY;
		private float _rotationZ;
		private float _rotationW;

		public InputRecord(Vector3 position, Quaternion rotation)
		{
			this._positionX = position.x;
			this._positionY = position.y;
			this._positionZ = position.z;
			this._rotationX = rotation.x;
			this._rotationY = rotation.y;
			this._rotationZ = rotation.z;
			this._rotationW = rotation.w;
		}

		public Vector3 Position
		{
			get { return new Vector3(_positionX, _positionY, _positionZ); }
		}

		public Quaternion Rotation
		{
			get { return new Quaternion(_rotationX, _rotationY, _rotationZ, _rotationW);}
		}
	}
}
