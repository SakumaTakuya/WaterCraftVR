using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sakkun.WatercraftVR.Game.UI
{
	public class SpeedMeter : MonoBehaviour 
	{
		[SerializeField, Range(1f, 100f)] private float _maxSpeed = 20f;
		[SerializeField] private Rigidbody _target;
		[SerializeField] private Image _gaugeImage;
		[SerializeField] private Text _speedText;

		private void Update()
		{
			// print(Time.fixedDeltaTime);
			var speed = _target.velocity.magnitude;
			_gaugeImage.fillAmount = speed / _maxSpeed;
			_speedText.text = speed.ToString("0.0");
		}
		
	}
}

