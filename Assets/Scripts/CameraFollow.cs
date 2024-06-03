using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private Vector3 _offset;

    [SerializeField] private bool _shouldFollowOnYAxis;
    [SerializeField] private float _dampening;

    private Vector3 _velocity = Vector3.zero;
    private float _originalY;
    private void Awake()
    {
        _originalY = transform.position.y;
        transform.position = GetTargetPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = GetTargetPosition();
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _dampening);
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = _playerTransform.position + _offset;
        targetPosition.z = transform.position.z;
        if (!_shouldFollowOnYAxis)
        {
            targetPosition.y = _originalY + _offset.y;
        }

        return targetPosition;
    }
}
