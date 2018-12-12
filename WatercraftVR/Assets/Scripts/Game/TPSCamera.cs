using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.WatercraftVR.Game
{
    public class TPSCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _lookOffset;


        private Transform _transform;
        private Vector3 _offset;


        private void Awake()
        {
            _transform = transform;
            _offset = _target.InverseTransformPoint(_transform.position);
        }

        private void FixedUpdate()
        {
            var pos = _target.TransformPoint(_offset);
            pos.y = _offset.y;
            _transform.position = pos;
            _transform.LookAt(_target.TransformPoint(_lookOffset));
        }
    }
}

