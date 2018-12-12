using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sakkun.Utility;
using System.Threading.Tasks;

namespace Sakkun.WatercraftVR.Game.Record
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int _rankingLength;
        [SerializeField] private string _rankingPath;
        [SerializeField] private string _savePath;
        private readonly List<TimeAttackData> _rankingData = new List<TimeAttackData>();
        private readonly List<TimeAttackData> _allData = new List<TimeAttackData>();

        public List<TimeAttackData> Data
        {
            get
            {
                if (_rankingData.Count == 0) Load();
                return _allData;
            }
        }

        private void Awake()
        {
            Load();
        }

        private void Load()
        {
            if (PlayerPrefs.HasKey(_savePath))
            {
                _allData.Add(
                    PlayerPrefsUtility.Load<TimeAttackData>(_savePath));
            }

            if (PlayerPrefs.HasKey(_rankingPath))
            {
                _rankingData.AddRange(
                    PlayerPrefsUtility.Load<List<TimeAttackData>>(_rankingPath));
            }

            _allData.AddRange(_rankingData);


            // foreach(var d in _rankingData)
            // {
            // 	print(d.FinishedTime+ "|" + d.Records.Count);
            // }
        }

        public void SaveRanking(TimeAttackData record)
        {
            _rankingData.Add(record);
            _rankingData.Sort((a, b) => (int)((a.FinishedTime - b.FinishedTime) * 1000));

            var len = Mathf.Min(_rankingLength, _rankingData.Count);
            PlayerPrefsUtility.Save<List<TimeAttackData>>(_rankingPath, _rankingData.Take(len).ToList());

        }

        public void SaveCurrent(TimeAttackData record)
        {
            PlayerPrefsUtility.Save<TimeAttackData>(_savePath, record);
        }
    }
}

