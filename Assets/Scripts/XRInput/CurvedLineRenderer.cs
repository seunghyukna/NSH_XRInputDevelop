using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Crengine.XRInput
{
	[RequireComponent(typeof(LineRenderer))]
	public class CurvedLineRenderer : MonoBehaviour
	{
		public float lineSegmentSize = 0.15f;
		public float lineWidth = 0.1f;
		[Header("Gizmos")]
		public bool showGizmos = true;
		public float gizmoSize = 0.1f;
		public Color gizmoColor = new Color(1, 0, 0, 0.5f);
		
		private CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
		private Vector3[] linePositions = new Vector3[0];
		private Vector3[] linePositionsOld = new Vector3[0];

		public void Update()
		{
			GetPoints();
			SetPointsToLine();
		}

		void GetPoints()
		{
			linePoints = this.GetComponentsInChildren<CurvedLinePoint>();

			linePositions = new Vector3[linePoints.Length];
			for (int i = 0; i < linePoints.Length; i++)
			{
				linePositions[i] = linePoints[i].transform.position;
			}
		}

		void SetPointsToLine()
		{
			if (linePositionsOld.Length != linePositions.Length)
			{
				linePositionsOld = new Vector3[linePositions.Length];
			}

			bool moved = false;

			for (int i = 0; i < linePositions.Length; i++)
			{
				if (linePositions[i] != linePositionsOld[i])
				{
					moved = true;
				}
			}

			if (moved == true)
			{
				LineRenderer line = this.GetComponent<LineRenderer>();

				Vector3[] smoothedPoints = SmoothLine(linePositions, lineSegmentSize);

				line.SetVertexCount(smoothedPoints.Length);
				line.SetPositions(smoothedPoints);
				line.SetWidth(lineWidth, lineWidth);
			}
		}

		private  Vector3[] SmoothLine(Vector3[] inputPoints, float segmentSize)
		{
			AnimationCurve curveX = new AnimationCurve();
			AnimationCurve curveY = new AnimationCurve();
			AnimationCurve curveZ = new AnimationCurve();

			Keyframe[] keysX = new Keyframe[inputPoints.Length];
			Keyframe[] keysY = new Keyframe[inputPoints.Length];
			Keyframe[] keysZ = new Keyframe[inputPoints.Length];

			for (int i = 0; i < inputPoints.Length; i++)
			{
				keysX[i] = new Keyframe(i, inputPoints[i].x);
				keysY[i] = new Keyframe(i, inputPoints[i].y);
				keysZ[i] = new Keyframe(i, inputPoints[i].z);
			}

			curveX.keys = keysX;
			curveY.keys = keysY;
			curveZ.keys = keysZ;

			for (int i = 0; i < inputPoints.Length; i++)
			{
				curveX.SmoothTangents(i, 0);
				curveY.SmoothTangents(i, 0);
				curveZ.SmoothTangents(i, 0);
			}

			List<Vector3> lineSegments = new List<Vector3>();

			for (int i = 0; i < inputPoints.Length; i++)
			{
				lineSegments.Add(inputPoints[i]);

				if (i + 1 < inputPoints.Length)
				{
					float distanceToNext = Vector3.Distance(inputPoints[i], inputPoints[i + 1]);

					int segments = (int)(distanceToNext / segmentSize);

					for (int s = 1; s < segments; s++)
					{
						float time = ((float)s / (float)segments) + (float)i;

						Vector3 newSegment = new Vector3(curveX.Evaluate(time), curveY.Evaluate(time), curveZ.Evaluate(time));

						lineSegments.Add(newSegment);
					}
				}
			}

			return lineSegments.ToArray();
		}

		#region gizmo

		void OnDrawGizmosSelected()
        {
            Update();
        }

        void OnDrawGizmos()
        {
            if (linePoints.Length == 0)
            {
                GetPoints();
            }

            foreach (CurvedLinePoint linePoint in linePoints)
            {
                linePoint.showGizmo = showGizmos;
                linePoint.gizmoSize = gizmoSize;
                linePoint.gizmoColor = gizmoColor;
            }
        }

        #endregion
    }
}