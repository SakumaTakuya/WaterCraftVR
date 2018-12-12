using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.Water
{
	//スペルミス : Ditector -> Detector
	public class WaterCollisionDitector : MonoBehaviour 
	{
		[SerializeField] private Camera _targetCamera;
		[SerializeField] private Shader _replaceShader;

		// Use this for initialization
		void Start () 
		{
			_targetCamera.SetReplacementShader(_replaceShader, null);
		}
	}
}



