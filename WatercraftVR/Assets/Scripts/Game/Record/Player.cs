using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.WatercraftVR.Game.Record
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GhostInput _input;
        [SerializeField] private ScoreManager _score;
        [SerializeField] private UI.WipeEffect _wipe;
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3f);
            while (true)
            {
                if (_score.Data.Count == 0) yield break;

                foreach (var data in _score.Data)
                {
                    _wipe.WipeIn(0.5f);
                    yield return new WaitForSeconds(1.5f);
                    _input.Play(data);
                    yield return new WaitForSeconds(1f);
                    _wipe.WipeOut(0.5f);
                    yield return new WaitForSeconds(data.FinishedTime - 5f);
                }
            }
        }
    }
}

