using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sakkun.WatercraftVR.Game.UI;

using static Unity.Mathematics.math;

namespace Sakkun.WatercraftVR.Game.WaterBike
{
    public class CapsizeDitector : MonoBehaviour
    {
        [SerializeField] private float _shutterTime = 1f;
        [SerializeField] private float _restoreTime = 0.1f;
        [SerializeField] private float _threshold = 170f;
        [SerializeField] private float _surface = 0f;

        [SerializeField] private Transform _target;

        [SerializeField] private WipeEffect _wipe;
        [SerializeField] private Navigator _navigator;

        private Rigidbody _rigidbody;
        private bool _isRestoring;

        private void Awake()
        {
            _rigidbody = _target.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_isRestoring || _target.position.y > _surface || Vector3.Angle(_target.up, Vector3.up) < _threshold) return;
            _isRestoring = true;



            _wipe.WipeIn(
                time: _shutterTime,
                endAction: () =>
                {
                    StartCoroutine(RestoreCoroutine());
                    // _rigidbody.velocity = Vector3.zero;
                });


        }

        private IEnumerator RestoreCoroutine()
        {
            var rate = _navigator.CurrentRate;

            var basePos = _target.position;
            var baseRot = _target.rotation;

            //位置を戻す
            var pos = _navigator.GetPoint(rate);
            pos.y = 0f;

            //方向を戻す
            var rot = Quaternion.LookRotation(_navigator.GetDirection(rate), Vector3.up);

            _rigidbody.isKinematic = true;


            var time = 0f;
            while (time < _restoreTime)
            {

                var t = time / _restoreTime;
                _rigidbody.position = Vector3.Lerp(basePos, pos, t);
                _rigidbody.rotation = Quaternion.Lerp(baseRot, rot, t);

                time += Time.deltaTime;

                yield return null;
            }
            _rigidbody.velocity = Vector3.zero;
            _wipe.WipeOut(_shutterTime);
            _isRestoring = false;
            _rigidbody.isKinematic = false;
        }
    }
}

