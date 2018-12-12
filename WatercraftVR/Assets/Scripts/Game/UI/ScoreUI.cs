using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sakkun.WatercraftVR.Game.Record;
using Sakkun.Utility;

namespace Sakkun.WatercraftVR.Game.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        [SerializeField] private ScoreManager _scoreManager;


        private void Start()
        {
            ShowScores();
        }
        private void ShowScores()
        {
            var s = "";
            foreach (var data in _scoreManager.Data.Indexed())
            {
                var time = data.Item().FinishedTime;
                s += data.Index().Ordinal("0;0;Previous") + "\t" + time.ToString("000.00") + "\n";
            }
            _scoreText.text = s;
        }
    }
}

