using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Collections;
using Unity.Jobs;

using static Unity.Mathematics.math;
using Unity.Mathematics;
using Sakkun.Utility;

namespace Sakkun.Water
{
    public class FloatingBody : MonoBehaviour
    {
        private struct FloatingJob : IJob
        {
            [ReadOnly] public NativeArray<Vector3> MassPoints; // 原点中心の質点
            [ReadOnly] public Matrix4x4 TRS;
            [ReadOnly] public float Density;
            [ReadOnly] public float Surface;
            [ReadOnly] public float Size;

            [WriteOnly] public NativeArray<float> Power;
            [WriteOnly] public NativeArray<Vector3> CenterOfBuoyancy;
            [WriteOnly] public NativeArray<float> SinkingRate;

            // private static int c = 0;

            public void Execute()
            {
                var centerOfBuoyancy = Vector3.zero;
                var count = 0;
                // c++;
                for (var i = 0; i < MassPoints.Length; i++)
                {
                    var worldPosition = TRS.MultiplyPoint3x4(MassPoints[i]);
                    // var surfacePoint = Surface.ClosestPointOnPlane(worldPosition);
                    // var direction = worldPosition - surfacePoint;
                    // var angle = Vector3.Angle(Surface.normal, direction);
                    // if(c==1) print("angle"+ angle);
                    // // 水面の点から質点へ向かうベクトルと、面の法線ベクトルが逆向きの時、その質点は水面下に存在しているといえる
                    // if(angle > 90f)
                    // {
                    //     count++;
                    //     centerOfBuoyancy += worldPosition;

                    // }


                    if (worldPosition.y < Surface)
                    {
                        //    if(c==1) print("angle"+ worldPosition);
                        count++;
                        centerOfBuoyancy += worldPosition;
                    }
                }

                Power[0] = Mathf.Pow(Size, 3) * count * 9.8f * Density;
                CenterOfBuoyancy[0] = centerOfBuoyancy * (count > 0 ? 1f / count : 0f);
                SinkingRate[0] = (float)count / MassPoints.Length;
            }
        }

#if UNITY_EDITOR
        [SerializeField] private bool _showMassPoint = true;
#endif

        [SerializeField] private float _drag = 0;
        [SerializeField] private float _waterDrag = 1.5f;
        [SerializeField] private float _anglerDrag = 0.05f;
        [SerializeField] private float _waterAnglerDrag = 0.15f;
        // [SerializeField] private float _displacement;
        [SerializeField] private Bounds _bounds;
        // [SerializeField] private RenderTexture _waterTexture;
        // [SerializeField] private Camera _dumpCamera;

        [SerializeField] private float _massRadius;
        // private Vector3 _normal;

        private Transform _transform;
        private Rigidbody _rigidbody;

        private FloatingJob _job;
        private JobHandle _jobHandle;

        private WaterVolume _water;

        public bool OnWater
        {
            get { return _water; }
        }

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            InitializeJob();
        }

        private void OnTriggerEnter(Collider other)
        {
            var water = other.GetComponent<WaterVolume>();
            if (water)
            {
                _water = water;

            }
        }

        private void OnTriggerStay()
        {
            if (_water)
            {
                AttachForce();
                SubscribeJob();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_water && _water.gameObject == other.gameObject) _water = null;
        }

        private void OnDisable()
        {
            DisposeJob();
        }

        private void OnDestroy()
        {
            DisposeJob();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            var trs = transform.GetTRS();
            var bounds = new Bounds();
            bounds.SetMinMax(trs.MultiplyPoint3x4(_bounds.min), trs.MultiplyPoint3x4(_bounds.max));
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            if (!_showMassPoint) return;
            foreach (var p in _job.MassPoints)
            {
                Gizmos.DrawSphere(trs.MultiplyPoint3x4(p), _massRadius);
            }
        }
#endif
        private void InitializeJob()
        {
            _job = new FloatingJob()
            {
                MassPoints = CalculateMassPoints(),
                Size = _massRadius,
                Power = new NativeArray<float>(1, Allocator.Persistent),
                CenterOfBuoyancy = new NativeArray<Vector3>(1, Allocator.Persistent),
                SinkingRate = new NativeArray<float>(1, Allocator.Persistent)
            };
        }

        private void SubscribeJob()
        {
            // _normal = Vector3.up; //CalculateNormal();
            _job.TRS = _transform.GetTRS();
            // print(_transform.GetTRS().MultiplyPoint3x4(Vector3.zero));
            _job.Density = _water.Density;
            _job.Surface = _water.Surface;//new Plane(_normal, Vector3.zero);
            _jobHandle = _job.Schedule();
        }

        private void AttachForce()
        {
            _jobHandle.Complete();
            // print(_job.Power[0] * _normal);
            // print(_job.CenterOfBuoyancy[0]);
            _rigidbody.AddForceAtPosition
            (
                _job.Power[0] * Vector3.up,
                _job.CenterOfBuoyancy[0]
            );
            _rigidbody.drag = lerp(_drag, _waterDrag, _job.SinkingRate[0]);
            _rigidbody.angularDrag = lerp(_anglerDrag, _waterAnglerDrag, _job.SinkingRate[0]);
        }

        private NativeArray<Vector3> CalculateMassPoints()
        {
            var size = _bounds.size / 2f;//Vector3.Scale(_bounds.size/2f, _transform.lossyScale);
            var massPoints = new List<Vector3>();
            var stride = _massRadius * 2f;
            for (var x = -size.x; x <= size.x; x += stride)
                for (var y = -size.y; y <= size.y; y += stride)
                    for (var z = -size.z; z <= size.z; z += stride)
                    {
                        //print(new Vector3(x, y, z));
                        massPoints.Add(new Vector3(x, y, z) + _bounds.center);
                    }

            // print(massPoints.Count);

            var nativeArray = new NativeArray<Vector3>
            (
                massPoints.Count,
                Allocator.Persistent
            );
            nativeArray.CopyFrom(massPoints.ToArray());
            return nativeArray;
        }

        // private Vector3 CalculateNormal()
        // {
        //     var cameraPos = _dumpCamera.WorldToViewportPoint(_transform.position);
        //     var texPosition = int2
        //     (
        //         (int)cameraPos.x  * _waterTexture.width,
        //         (int)cameraPos.y * _waterTexture.height
        //     );
        //     var texture = _waterTexture.GetTexture();

        //     //     +---+
        //     //     | C |
        //     // +---+---+---+
        //     // | A |   | B |
        //     // +---+---+---+
        //     //     | D |
        //     //     +---+
        //     var colA = texture.GetPixel(texPosition.x+1, texPosition.y);
        //     var colB = texture.GetPixel(texPosition.x-1, texPosition.y);
        //     var colC = texture.GetPixel(texPosition.x, texPosition.y+1);
        //     var colD = texture.GetPixel(texPosition.x, texPosition.y-1);

        //     return Vector3.Cross(float3(2, 0, colA.r-colB.r), float3(0, 2, colC.r - colD.r));
        // }

        private void DisposeJob()
        {
            if (!_job.MassPoints.IsCreated) return;
            _jobHandle.Complete();

            _job.MassPoints.Dispose();
            _job.Power.Dispose();
            _job.CenterOfBuoyancy.Dispose();
            _job.SinkingRate.Dispose();
        }

    }
}

