using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sakkun.Utility;

namespace Sakkun.Water
{
	[ExecuteInEditMode]
	public class UnderWaterEffect : MonoBehaviour 
	{
		[SerializeField] private Camera _targetCamera; 
		[SerializeField] private Material _effectMaterial;

		// private RenderTexture _colorTexture;
		// private RenderTexture _depthTexture;

		// private int _cameraTRSID;
		// private Transform _cameraTransform;

		private void Awake()
		{
			// _cameraTransform = _targetCamera.transform;
			// _cameraTRSID = Shader.PropertyToID("_CameraTRS");
			_targetCamera.depthTextureMode |= DepthTextureMode.Depth;
			// InitDepth();
			// AddDepthCommand();
		}

		private void OnRenderImage(RenderTexture src, RenderTexture dest) {
			
			// _effectMaterial.SetMatrix("_CameraTRS", Matrix4x4.Rotate(_cameraTransform.rotation));
			Graphics.Blit(src, dest, _effectMaterial);
		}

	// 	private void InitDepth()
	// 	{
	// 		//カラーバッファを生成
	// 		_colorTexture = new RenderTexture(Screen.width, Screen.height, 0);
	// 		_colorTexture.Create();
			
	// 		//デプスバッファを生成
	// 		_depthTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Depth);
	// 		_depthTexture.Create();

	// 		_targetCamera.SetTargetBuffers(_colorTexture.colorBuffer, _depthTexture.depthBuffer);
	// 	}

	// 	private void AddDepthCommand()
	// 	{
	// 		{
	// 			var command = new CommandBuffer();
	// 			command.name = "Set depth texture";
	// 			command.SetGlobalTexture("_DepthTexture", _depthTexture);
	// 			_targetCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, command);
	// 		}

	// 		{
	// 			var command = new CommandBuffer();
	// 			command.name = "Blit to back buffer";
	// 			command.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
	// 			command.SetGlobalTexture("_MainTexture", _colorTexture);
	// 			// command.Blit(_colorTexture, BuiltinRenderTextureType.CurrentActive);
	// 			_targetCamera.AddCommandBuffer(CameraEvent.AfterImageEffects, command);
	// 		}

	// 	}
	}
}

