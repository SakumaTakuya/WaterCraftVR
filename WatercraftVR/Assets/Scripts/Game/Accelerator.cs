using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.WatercraftVR.Game
{
    public class Accelerator : MonoBehaviour
    {
        [SerializeField] private float _waitAngle = 5f;
        [SerializeField] private float _power = 1000f;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private WaterBike.Engine _engine;

        private void OnTriggerEnter(Collider other)
        {
            // var body = other.GetComponent<Rigidbody>();
            // if (body) StartCoroutine(Accelerate(body));

            if (other.gameObject == _engine.gameObject)
            {
                _engine.Speed *= _power;
                _particle.Emit(50);
                _audioSource.Play();
            }



        }

        // private IEnumerator Accelerate(Rigidbody body)
        // {
        //     var tr = body.transform;
        //     var forward = tr.forward;
        //     // var rot = Quaternion.LookRotation(forward, Vector3.up);
        //     yield return new WaitUntil(() =>
        //     {
        //         forward = tr.forward;
        //         forward.y = 0f;
        //         return Vector3.Angle(forward, body.velocity) < _waitAngle;
        //     });
        //     body.AddForce(forward * _power, ForceMode.Impulse);
        //     // print("acc");

        // }
    }
}

