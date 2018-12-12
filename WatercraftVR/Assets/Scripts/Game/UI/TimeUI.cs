using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sakkun.WatercraftVR.Game.UI
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private float _fadeTime = 0.5f;
        [SerializeField] private Text _startText;

        private Color _textColor;
        private Color _fadeColor;

        private void Awake()
        {
            _textColor = _startText.color;
            _fadeColor = _textColor;
            _fadeColor.a = 0f;
        }

        public IEnumerator ShowCountDown(int num)
        {
            var time = 0f;
            _startText.text = num.ToString("0;0;Start!");
            while (time < _fadeTime)
            {
                _startText.color = Color.Lerp(_textColor, _fadeColor, time / _fadeTime);
                time += Time.deltaTime;
                yield return null;
            }
        }

        public void VanishStartText()
        {
            _startText.gameObject.SetActive(false);
        }

    }
}

