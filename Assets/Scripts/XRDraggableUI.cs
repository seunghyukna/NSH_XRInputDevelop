using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using XRRayHit = XRInput.Core.XRRayHitPosition;

public class XRDraggableUI : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster graphicRaycaster;

    private void Start()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    private void Update()
    {
        var ped = new PointerEventData(null);
        ped.position = XRRayHit.Instance.HitPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(ped, results);

        if (results.Count <= 0) return;
        // 이벤트 처리부분
        results[0].gameObject.transform.position = ped.position;
        Debug.Log(results[0]);
    }

    private void DetectRay()
    {
        
        Debug.Log("Hit Ray");
    }
}
