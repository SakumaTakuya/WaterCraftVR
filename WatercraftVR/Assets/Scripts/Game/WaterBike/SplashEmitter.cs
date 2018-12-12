using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sakkun.Water;
using static Unity.Mathematics.math;

namespace Sakkun.WatercraftVR.Game.WaterBike
{
    public class SplashEmitter : MonoBehaviour
    {
        [SerializeField] private float _surface;
        [SerializeField] private float _maxRate = 20f;
        [SerializeField] private ParticleSystem[] _particles;
        [SerializeField] private AudioSource _audioSource;
        // private ParticleSystem.MainModule[] _mains;
        private ParticleSystem.EmissionModule[] _emissions;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            // _mains = new ParticleSystem.MainModule[_particles.Length];
            _emissions = new ParticleSystem.EmissionModule[_particles.Length];
            for (var i = 0; i < _particles.Length; i++)
            {
                // _mains[i] = _particles[i].main;
                _emissions[i] = _particles[i].emission;
            }
        }

        private void Update()
        {
            var emitRate = _rigidbody.position.y < _surface ?
                min(floor(_rigidbody.velocity.magnitude * 2f), _maxRate) : 0f;
            EmitSplash(emitRate);
        }

        private void EmitSplash(float emitRate)
        {
            _audioSource.volume = emitRate / _maxRate;
            for (var i = 0; i < _emissions.Length; i++)
            {
                _emissions[i].rateOverTime = emitRate;
            }
        }
    }
}

