using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakkun.WatercraftVR.Game.Scene
{
    public class StartScene : MonoBehaviour
    {
        [SerializeField] private SceneChanger _sceneChaner;

        [SerializeField] private GameObject _canvas;

        // private void Update()
        // {
        //     if (Input.anyKeyDown) _sceneChaner.SceneChange(1);
        // }

        public void StartGame(int index)
        {
            _sceneChaner.SceneChange(index);
            _canvas.SetActive(false);
        }
    }
}

