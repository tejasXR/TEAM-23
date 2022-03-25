using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScale : MonoBehaviour
{
    [SerializeField] private float scaleAmount;
    [SerializeField] private float smoothingTime;
    
    private Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        var distance = Vector3.Distance(transform.position, _player.PlayerHead.transform.position);
        var newScale = Vector3.one * distance * scaleAmount;
        transform.localScale =  Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * smoothingTime);
    }
}
