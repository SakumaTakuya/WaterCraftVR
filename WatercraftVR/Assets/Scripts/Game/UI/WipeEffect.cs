using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Sakkun.WatercraftVR.Game.UI
{
    public class WipeEffect : MonoBehaviour
    {
        [SerializeField] private Material _wipeEffect;

        private int _radiusID;

        private bool _isWipe;

        private void Awake()
        {
            _radiusID = Shader.PropertyToID("_Radius");
            _wipeEffect.SetFloat(_radiusID, 2f);
        }
        public void WipeIn(float time, System.Action<float> intervalAction = null, System.Action endAction = null)
        {
            StartCoroutine(WipeEnumrator(time, false, intervalAction, endAction));
        }

        public void WipeOut(float time, System.Action<float> intervalAction = null, System.Action endAction = null)
        {
            StartCoroutine(WipeEnumrator(time, true, intervalAction, endAction));
        }

        private IEnumerator WaitWipe(float time, bool isOut, System.Action<float> intervalAction, System.Action endAction = null)
        {
            yield return new WaitUntil(() => !_isWipe);
            StartCoroutine(WipeEnumrator(time, isOut, intervalAction, endAction));
        }

        private IEnumerator WipeEnumrator(float time, bool isOut, System.Action<float> intervalAction, System.Action endAction = null)
        {
            if (_isWipe)
            {
                StartCoroutine(WaitWipe(time, isOut, intervalAction, endAction));
                yield break;
            }

            _isWipe = true;

            var t = 0f;
            var doubleTime = time * 2f;
            while (t < doubleTime)
            {
                // print(t);
                intervalAction?.Invoke(t / doubleTime);
                _wipeEffect.SetFloat(_radiusID, isOut ? t / time : 2f - t / time);
                t += Time.deltaTime;
                yield return null;
            }
            _wipeEffect.SetFloat(_radiusID, isOut ? 2f : 0f);
            endAction?.Invoke();

            _isWipe = false;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {

            // _effectMaterial.SetMatrix("_CameraTRS", Matrix4x4.Rotate(_cameraTransform.rotation));
            Graphics.Blit(src, dest, _wipeEffect);
        }
    }
}

