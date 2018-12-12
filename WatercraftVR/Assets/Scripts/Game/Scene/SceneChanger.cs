using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sakkun.WatercraftVR.Game.UI;

namespace Sakkun.WatercraftVR.Game.Scene
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private WipeEffect _wipe;

        private float _wipeTime = 0.5f;

        private void Start()
        {
            _wipe.WipeOut(_wipeTime);
        }

        public void SceneChange(int index)
        {
            _wipe.WipeIn(
                _wipeTime,
                endAction: () =>
                {
                    SceneManager.LoadScene(index);
                }
            );
        }
    }
}

