using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.WatercraftVR.Game.Scene
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] private SceneChanger _sceneChanger;
        [SerializeField] private Navigator _naviagetor;
        private void Start()
        {

        }

        private void Update()
        {

            if
            (
                Input.GetKeyDown(KeyCode.Escape) ||
                (Input.anyKeyDown && _naviagetor.IsGoal)
            )
                _sceneChanger.SceneChange(0);
        }
    }
}

