using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sakkun.Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sakkun.WatercraftVR.Game
{
    public class Navigator : MonoBehaviour
    {

        [SerializeField] private Path _path;
        [SerializeField] private float _reverseAngle = 100f;
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _ordinaryPoint;
        [SerializeField] private Vector3 _displayPoint;
        [SerializeField] private Chaser _chaser;

        private int _current;
        private bool[] _isPassed;

        public int CurrentPath
        {
            get { return _current; }
            set { _current = (_path.Segments.Length + value) % _path.Segments.Length; }
        }

        public float CurrentRate
        {
            get
            {
                var cur = _path.Segments[_current];
                var startDist = Vector3.Distance(cur.P0, _target.position);
                var endDist = Vector3.Distance(cur.P3, _target.position);
                return startDist / (startDist + endDist);
            }
        }

        public bool IsGoal
        {
            get
            {
                for (var i = 0; i < _isPassed.Length; i++)
                {
                    if (_isPassed[i])
                    {
                        continue;
                    }
                    return false;
                }
                return true;
            }
        }

        private bool IsPassedEndPoint(EndPoint point)
        {
            var isEndPoint = point == EndPoint.End;
            var segment = _path.Segments[_current];
            var endPlane = isEndPoint ?
                            new Plane(segment.GetGradient(1), segment.P3) :
                            new Plane(-segment.GetGradient(0), segment.P0);

            var ret = endPlane.GetSide(_target.position);//&& Vector3.Distance(isEndPoint ? segment.P3 : segment.P0, _target.position) < 30;

            _isPassed[_current] |= isEndPoint && ret;
            return ret;
        }

        public bool IsReverseRunning()
        {
            // print(Vector3.Angle(tr.forward, dir));
            return IsReverse(_target.forward, GetDirection());
        }

        public bool IsReverse(Vector3 from, Vector3 to)
        {
            return Vector3.Angle(from, to) > _reverseAngle;
        }

        public Vector3 GetDirection()
        {
            var rate = CurrentRate;
            return GetDirection(rate);
        }

        public Vector3 GetDirection(float rate)
        {
            var cur = _path.Segments[_current];
            return cur.GetGradient(rate);
        }

        public Vector3 GetPoint()
        {
            var rate = CurrentRate;
            return GetPoint(rate);
        }

        public Vector3 GetPoint(float rate)
        {
            var cur = _path.Segments[_current];
            return cur.GetPoint(rate);
        }



        private void Awake()
        {
            _isPassed = new bool[_path.Segments.Length];
        }

        private void Update()
        {
            // print(CurrentPath);
            // print(IsGoal);
            var isReverse = IsReverseRunning();

            var endPoint = isReverse ? EndPoint.Start : EndPoint.End;
            if (IsPassedEndPoint(endPoint))
            {
                CurrentPath += (int)endPoint;
            }

            _chaser.Distination = isReverse ? _displayPoint : _ordinaryPoint;
        }

#if UNITY_EDITOR
		[CustomEditor(typeof(Navigator)), CanEditMultipleObjects]
		public class DrawCourse : Editor
		{
			private void OnSceneViewGUI(SceneView sv)
			{
				Navigator course = target as Navigator;

				Undo.RecordObject(course, "Change pathes of course");
				for(var i = 0; i < course._path.Segments.Length; i++)
				{
					var be = course._path.Segments[i];
					course._path.Segments[i].P0 = Handles.PositionHandle(be.P0, Quaternion.identity);
					course._path.Segments[i].P1 = Handles.PositionHandle(be.P1, Quaternion.identity);
					course._path.Segments[i].P2 = Handles.PositionHandle(be.P2, Quaternion.identity);
					course._path.Segments[i].P3 = Handles.PositionHandle(be.P3, Quaternion.identity);

					Handles.DrawBezier(be.P0, be.P3, be.P1, be.P2, Color.red, null, 2f);
				}


			}

			void OnEnable()
			{
				// Debug.Log("OnEnable");
				SceneView.onSceneGUIDelegate += OnSceneViewGUI;
			}

			void OnDisable()
			{
				// Debug.Log("OnDisable");
				SceneView.onSceneGUIDelegate -= OnSceneViewGUI;
			}
		}
		
		private void OnDrawGizmosSelected()
        {
			if(!_target) return;
            Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(_target.TransformPoint(_ordinaryPoint), 0.5f);
            Gizmos.color = Color.cyan;
			Gizmos.DrawSphere(_target.TransformPoint(_displayPoint), 0.5f);

			foreach(var segment in _path.Segments)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(segment.P0, segment.P0 - segment.GetGradient(0));
            	Gizmos.color = Color.white;
				Gizmos.DrawLine(segment.P3, segment.P3 + segment.GetGradient(1));
			}
			
        }
#endif
    }

    public enum EndPoint
    {
        Start = -1,
        End = 1
    }
}
