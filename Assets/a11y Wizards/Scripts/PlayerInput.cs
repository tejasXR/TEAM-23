using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInput : MonoBehaviour
{
    // [SerializeField] private GameObject playerHead;
    [Space]
    [SerializeField] private AnnotationSystem annotationSystem;
    [SerializeField] private OVRInput.Button annotationButton;

    private GameObject _playerHead;
    private void Start()
    {
        _playerHead = GetComponent<Player>().PlayerHead;
    }

    private void FixedUpdate()
    {
        OVRInput.Update();
        CreateAnnotationButton();
    }

    private void CreateAnnotationButton()
    {
        if (OVRInput.GetDown(annotationButton))
        {
            annotationSystem.CreateAnnotation("", HeadRaycastPosition());
        }
    }

    private Vector3 HeadRaycastPosition()
    {
        var ray = new Ray(_playerHead.transform.position, _playerHead.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                return hit.point;
            }
        }
        
        return _playerHead.transform.forward;
    }
}
