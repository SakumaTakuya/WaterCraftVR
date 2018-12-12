using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sakkun.Utility;

using static Unity.Mathematics.math;

namespace Sakkun.WatercraftVR.Game.WaterBike
{
    [RequireComponent(typeof(Rigidbody))]
    public class Engine : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _enginPoint;
        [SerializeField] private Vector3 _handlePoint;
        [SerializeField] private float _handleSentitivity;

        private Rigidbody _rigidbody;
        private Transform _transform;

        public Vector3 CenterOfMass
        {
            get { return _rigidbody.centerOfMass; }
            set { _rigidbody.centerOfMass = value; }
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public float Handle { get; set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = transform;
        }

        public void Run(float power)
        {
            _rigidbody.AddForceAtPosition
            (
                _transform.forward * power * _speed,
                _transform.TransformPoint(_enginPoint)
            );

            _rigidbody.AddForceAtPosition
            (
                -transform.forward * power * Handle * _handleSentitivity,
                _transform.TransformPoint(_handlePoint)
            );
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.TransformPoint(_enginPoint), 0.2f);
            Gizmos.DrawSphere(transform.TransformPoint(_handlePoint), 0.2f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.TransformPoint(GetComponent<Rigidbody>().centerOfMass), 0.2f);
        }
#endif
    }

}
