using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sakkun.WatercraftVR.Game.Record;
using Sakkun.Utility;

namespace Sakkun.WatercraftVR.Game.UI
{
    public class RecordUI : MonoBehaviour
    {
        [SerializeField] private float _easeTime = 1f;
        [SerializeField] private Timer _timer;
        [SerializeField] private Text _secText;
        [SerializeField] private Text _mirisecText;
        [SerializeField] private Text _goalText;

        private void Start()
        {
            _goalText.gameObject.SetActive(false);
            _timer.StopEvent += ShowGoal;
        }

        private void Update()
        {
            if (!_timer.IsWorking) return;

            var now = _timer.Now;
            var sec = (int)now;
            _secText.text = sec.ToString("000");
            _mirisecText.text = Mathf.Clamp(now - sec, 0f, 0.99f).ToString(".00");
        }

        private void ShowGoal(float time)
        {
            StartCoroutine(ShowGoalEnumrator());
        }

        private IEnumerator ShowGoalEnumrator()
        {
            _goalText.gameObject.SetActive(true);
            var time = 0f;
            var col = _goalText.color;
            while (time < _easeTime)
            {
                _goalText.color = Color.Lerp(Color.clear, col, time / _easeTime);

                time += Time.deltaTime;
                yield return null;
            }
            _goalText.color = col;
        }
    }
}

