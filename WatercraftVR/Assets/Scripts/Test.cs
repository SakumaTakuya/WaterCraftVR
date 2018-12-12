using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour 
{
	[SerializeField] private MeshFilter _filter;
	[SerializeField] private Bounds _bounds;

	private Mesh _mesh;

	// Use this for initialization
	void Start () {
		// _mesh = _filter.mesh;
		// foreach(var norm in _mesh.normals)
		// {
		// 	print(norm);
		// }
		// print(_mesh.normals.Length);
		// print(_mesh.vertices.Length);

		
		print(Camera.main.pixelWidth + "," + Camera.main.pixelHeight);
	}
	
	// Update is called once per frame
	void Update () {
print(Camera.main.WorldToViewportPoint(transform.position));
	}

	private void OnGUI()
	{
		
	}

	private void OnDrawGizmosSelected()
	{
		if(!_filter) _filter = GetComponent<MeshFilter>();
		Gizmos.color = Color.green;
		
		var trs = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		var bounds = _filter.sharedMesh.bounds;
		bounds.SetMinMax(trs.MultiplyPoint3x4(bounds.min), trs.MultiplyPoint3x4(bounds.max));
		Gizmos.DrawWireCube(bounds.center, bounds.size);

		// Gizmos.color = Color.blue;
		// var col_bounds = GetComponent<Collider>().bounds;
		// Gizmos.DrawWireCube(col_bounds.center, col_bounds.size);
	}

	// private void OnDrawGizmosSelected() 
	// {
	// 	Gizmos.color = Color.green;
	// 	for(int i = 0; i < _filter.mesh.normals.Length; i++)
	// 	{
	// 		Gizmos.DrawLine
	// 		(
	// 			_filter.mesh.vertices[i] + transform.position,
	// 			_filter.mesh.normals[i] + _filter.mesh.vertices[i] + transform.position
	// 		);
	// 	}
    // }
}
