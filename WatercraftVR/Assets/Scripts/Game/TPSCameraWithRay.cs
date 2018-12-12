using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TPSCameraWithRay : MonoBehaviour
{
	public Transform Target;
	[SerializeField] private float _speed = 1;
//	[SerializeField] private float _roteSpeed = 6;
	[SerializeField] private Vector2 _sensitivity = new Vector2(15, 15);
	[SerializeField] private Vector2 _min = new Vector2(-360, -60);
	[SerializeField] private Vector2 _max = new Vector2(360, 60);

	[SerializeField] private Transform _axis;
	[SerializeField] private Transform _pivot;

	private Transform _myTransform;

	//private Quaternion _offsetRot;
	private Vector3 _offsetPos;

	private float _roteX;
	private float _roteY;

	private bool _onWall;
	private Vector3 _wallNormal;

	private Collider _targetCollider;

	private float _distance;

	// Use this for initialization
	private void Start()
	{
		_myTransform = transform;
		//_offsetRot = _pivot.localRotation;
		_offsetPos = _myTransform.position - _pivot.position;

	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		_roteX += Input.GetAxis("Mouse X") * _sensitivity.x;
		_roteY += Input.GetAxis("Mouse Y") * _sensitivity.y;
		_roteX = ClampAngle(_roteX, _min.x, _max.x);
		_roteY = ClampAngle(_roteY, _min.y, _max.y);

		_pivot.localRotation = Quaternion.Euler(-_roteY, _roteX, 0);
		// _axis.rotation = Quaternion.FromToRotation(_axis.up, Target.up) * _axis.rotation;
		_myTransform.rotation = _pivot.rotation;


		_axis.position = Vector3.Lerp(
			_axis.position,
			Target.position,
			Time.fixedDeltaTime * _speed
		);
		
		_myTransform.position = _pivot.TransformPoint(_offsetPos);
		
		RaycastHit hit;
		var ray = new Ray(_pivot.position, _myTransform.position - _pivot.position);
		if (Physics.SphereCast(ray, 0.1f, out hit, Mathf.Infinity,~LayerMask.GetMask("Player")) && hit.transform != _myTransform)
		{
			Debug.DrawLine(_pivot.position, hit.point, Color.yellow);
			_myTransform.position = hit.point - (_myTransform.position - _pivot.position).normalized;
		}

	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360) angle += 360;
		if (angle > 360) angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}