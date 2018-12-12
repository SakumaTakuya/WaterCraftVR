using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.Water
{
	public class WaterVolume : MonoBehaviour 
	{
		[SerializeField] private float _density = 1f;

		private float _surface;
		public float Density
		{
			get{ return _density;}
		}

		public float Surface
		{
			get{return _surface;}
		}

		private void Awake()
		{
			_surface = transform.position.y;
		}
	}
}

