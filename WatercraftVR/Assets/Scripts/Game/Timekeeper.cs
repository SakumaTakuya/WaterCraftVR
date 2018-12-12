using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sakkun.WatercraftVR.Game.Record;
using Sakkun.WatercraftVR.Game.UI;

namespace Sakkun.WatercraftVR.Game
{
    public class Timekeeper : MonoBehaviour
    {
        [SerializeField] private float _waitTime = 0.2f;
        [SerializeField] private Timer _timer;
        [SerializeField] private Navigator _navigator;
        [SerializeField] private TimeUI _ui;
        [SerializeField] private AudioSource _audioSource;

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(1f);
            _audioSource.Play();
            yield return new WaitForSecondsRealtime(_waitTime);


            var count = 3;
            while (count > 0)
            {
                StartCoroutine(_ui.ShowCountDown(count--));
                yield return new WaitForSecondsRealtime(1f);
            }

            StartCoroutine(_ui.ShowCountDown(0));
            _timer.StartTimer();

            yield return new WaitForSeconds(1f);
            _ui.VanishStartText();
        }

        private void Update()
        {

            if (_navigator.IsGoal)
            {
                _timer.StopTimer();
                return;
            }
        }
    }
}

