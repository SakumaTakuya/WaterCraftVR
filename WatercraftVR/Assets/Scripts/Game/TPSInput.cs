using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sakkun.WatercraftVR.Game.WaterBike;
using Sakkun.WatercraftVR.Game.Record;

using static Unity.Mathematics.math;

namespace Sakkun.WatercraftVR.Game
{
    public class TPSInput : MonoBehaviour
    {
        [SerializeField] private Engine _engine;
        [SerializeField] private float _centerRadius;
        [SerializeField] private Vector3 _centerOffset;
        [SerializeField] private Vector2 _sensitivity;
        [SerializeField] private Timer _timer;

        [SerializeField] private bool _isEasy;

        private Vector2 _centerOfMass = Vector2.zero;

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (_timer.IsWorking) _engine.Run(Input.GetAxis("Jump"));

            _engine.Handle = Input.GetAxis("Horizontal");
            _centerOfMass.x += Input.GetAxis("Mouse X") * _sensitivity.x;
            _centerOfMass.y += Input.GetAxis("Mouse Y") * _sensitivity.y;
            _centerOfMass.x = clamp(Input.mousePosition.x / Screen.width * 2f - 1f, -_centerRadius, _centerRadius);
            _centerOfMass.y = clamp(Input.mousePosition.y / Screen.height * 2f - 1f, -_centerRadius, _centerRadius);
            // print(_centerOfMass);
            // _engine.CenterOfMass = new Vector3(_centerOfMass.x, , _centerOfMass.y) + _centerOffset;
            var h = _isEasy ? -1f : -(_centerOfMass.x * _centerOfMass.x + _centerOfMass.y * _centerOfMass.y);
            _engine.CenterOfMass = new Vector3(_centerOfMass.x, h, _centerOfMass.y) + _centerOffset;

        }
    }
}

