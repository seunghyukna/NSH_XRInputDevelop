﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezeirTest : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints;

    private Vector3 gizmoPosition;

    private void OnDrawGizmos()
    {
        for (float i = 0; i < 1; i += 0.05f)
        {
            gizmoPosition = Mathf.Pow(1 - i, 3) * controlPoints[0].position +
                3 * Mathf.Pow(1 - i, 2) * i * controlPoints[1].position +
                3 * (1 - i) * Mathf.Pow(i, 2) * controlPoints[2].position +
                Mathf.Pow(i, 3) * controlPoints[3].position;

            Gizmos.DrawSphere(gizmoPosition, 0.25f);
        }

        Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y),
            new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));
        Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y),
            new Vector2(controlPoints[3].position.x, controlPoints[1].position.y));
    }
}
