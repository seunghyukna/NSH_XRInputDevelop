using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardTest : MonoBehaviour
{
    public Transform player;
    public Transform target;

    private void Update()
    {
        transform.LookAt(player.position + transform.rotation * Vector3.forward, transform.rotation * Vector3.up);
        Vector3 angle = transform.eulerAngles;
        angle.x = 0;
        transform.eulerAngles = angle;
    }
}
