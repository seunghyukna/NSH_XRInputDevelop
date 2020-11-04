using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Crengine.XRInput
{
    [RequireComponent(typeof(BoxCollider))]
    public class XRObjectManipulater : MonoBehaviour
    {

        private XRBaseInteractable interactable;
        [SerializeField] private GameObject boundingBoxPrefab;
        [SerializeField] private GameObject scalePrefab;
        [SerializeField] private GameObject rotatePrefab;
        private BoxCollider boxCollider;
        private GameObject rig;
        private GameObject boundingBox;
        private float scalePrefabScale = 0.1f;
        private float rotatePrefabScale = 0.1f;

        [Header("Scale")]
        [SerializeField] private float minScale = 0.1f;
        [SerializeField] private float maxScale = 2f;
        private Vector3[] scalePoints;
        private GameObject[] scaleObjects;

        [Header("Ratate")]
        private Vector3[] rotatePoints;
        private GameObject[] rotateObjects;

        private void Start()
        {
            interactable = GetComponent<XRBaseInteractable>();
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            InitializeBoudingBox();
        }

        private void Update()
        {
            SwitchActive();
        }

        private void SwitchActive()
        {
            if (interactable.isHovered)
            {
                OnOffChilds(true);
                Syncronization();
            }
            else
                OnOffChilds(false);
        }

        private void OnOffChilds(bool _flag)
        {
            foreach (Transform child in rig.transform)
            {
                child.gameObject.SetActive(_flag);
            }
        }

        private void Syncronization()
        {
            rig.transform.position = transform.position;
            boundingBox.transform.localPosition = boxCollider.center;

            // test rotation
            //transform.RotateAround(boxCollider.bounds.center, transform.forward, 1f);

        }

        private void InitializeBoudingBox()
        {
            rig = new GameObject("Rig");
            boundingBox = new GameObject("BoundingBox");
            rig.transform.parent = transform;
            rig.transform.position = transform.position + boxCollider.center;
            rig.transform.rotation = Quaternion.Euler(transform.eulerAngles);

            boundingBox = PrefabUtility.InstantiatePrefab(boundingBoxPrefab) as GameObject;
            boundingBox.transform.parent = rig.transform;
            boundingBox.transform.localScale = boxCollider.size;
            boundingBox.transform.position = rig.transform.position;
            boundingBox.AddComponent<CursorContext>();
            boundingBox.GetComponent<CursorContext>().CurrentCursorAction = CursorContext.CursorAction.Move; 

            InitializeEdgePoint(boundingBox);
            InitializeMidPoint(boundingBox);
        }

        private void InitializeEdgePoint(GameObject _box)
        {
            scaleObjects = new GameObject[8];
            scalePoints = GetBoxColliderVertexPositions();

            for (int i = 0; i < scalePoints.Length; i++)
            {
                GameObject scaleObject = Instantiate(scalePrefab, transform.position, transform.rotation);
                scaleObject.transform.position = scalePoints[i];
                scaleObject.transform.parent = _box.transform;
                scaleObject.name = "scale_" + i;
                scaleObject.AddComponent<BoxCollider>();
                scaleObject.AddComponent<CursorContext>();
                scaleObject.GetComponent<CursorContext>().CurrentCursorAction = CursorContext.CursorAction.Scale;
                scaleObject.GetComponent<BoxCollider>().isTrigger = true;
                scaleObject.GetComponent<BoxCollider>().size = new Vector3(2, 2, 2);

                scaleObjects[i] = scaleObject;
                CalculateEdgePointRotation(scaleObject, i);
            }
        }

        private Vector3[] GetBoxColliderVertexPositions()
        {
            BoxCollider b = boxCollider;
            Vector3[] vertices = new Vector3[8];

            vertices[0] = transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f);
            vertices[1] = transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f);
            vertices[2] = transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, -b.size.z) * 0.5f);
            vertices[3] = transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, b.size.z) * 0.5f);
            vertices[4] = transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, -b.size.z) * 0.5f);
            vertices[5] = transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, b.size.z) * 0.5f);
            vertices[6] = transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f);
            vertices[7] = transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f);

            return vertices;
        }

        private void CalculateEdgePointRotation(GameObject _object, int _index)
        {
            float x, y, z;

            if (_index > 3) x = -1;
            else x = 1;

            if (_index < 2 || _index > 5) y = -1;
            else y = 1;

            if (_index % 2 == 0) z = -1;
            else z = 1;

            _object.transform.localScale = 
                new Vector3(scalePrefabScale * x, scalePrefabScale * y, scalePrefabScale * z);
        }

        private void InitializeMidPoint(GameObject _box)
        {
            rotatePoints = new Vector3[12];
            rotateObjects = new GameObject[12];
            CalculateMidPointPosition();

            for (int i = 0; i < rotatePoints.Length; i++)
            {
                GameObject rotateObject = Instantiate(rotatePrefab, transform.position, transform.rotation);
                rotateObject.transform.position = rotatePoints[i];
                rotateObject.transform.localScale = new Vector3(rotatePrefabScale, rotatePrefabScale, rotatePrefabScale);
                rotateObject.transform.parent = _box.transform;
                rotateObject.name = "rotate_" + i;
                rotateObject.AddComponent<BoxCollider>();
                rotateObject.AddComponent<CursorContext>();
                rotateObject.GetComponent<CursorContext>().CurrentCursorAction = CursorContext.CursorAction.Rotate;
                rotateObject.GetComponent<BoxCollider>().isTrigger = true;
                rotateObject.GetComponent<BoxCollider>().size = new Vector3(0.5f, 1, 0.5f);

                rotateObjects[i] = rotateObject;

                CalculateMidPointRotation(rotateObject, i);
            }
        }

        private void CalculateMidPointPosition()
        {
            rotatePoints[0] = (scalePoints[0] + scalePoints[1]) * 0.5f;
            rotatePoints[1] = (scalePoints[0] + scalePoints[2]) * 0.5f;
            rotatePoints[2] = (scalePoints[3] + scalePoints[2]) * 0.5f;
            rotatePoints[3] = (scalePoints[3] + scalePoints[1]) * 0.5f;

            rotatePoints[4] = (scalePoints[4] + scalePoints[5]) * 0.5f;
            rotatePoints[5] = (scalePoints[4] + scalePoints[6]) * 0.5f;
            rotatePoints[6] = (scalePoints[7] + scalePoints[6]) * 0.5f;
            rotatePoints[7] = (scalePoints[7] + scalePoints[5]) * 0.5f;

            rotatePoints[8] = (scalePoints[0] + scalePoints[6]) * 0.5f;
            rotatePoints[9] = (scalePoints[1] + scalePoints[7]) * 0.5f;
            rotatePoints[10] = (scalePoints[2] + scalePoints[4]) * 0.5f;
            rotatePoints[11] = (scalePoints[3] + scalePoints[5]) * 0.5f;
        }

        private void CalculateMidPointRotation(GameObject _object, int _index)
        {
            if (_index < 8)
            {
                if (_index % 2 == 0)
                    _object.transform.rotation = Quaternion.Euler(90, 0, 0);
                else
                    _object.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
                _object.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
}