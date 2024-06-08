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
    [SerializeField] private float _dampeningX;
    [SerializeField] private float _dampeningY;

    private Vector3 _velocity = Vector3.zero;
    private float _originalY;
    private void Awake()
    {
        _originalY = transform.position.y;
        transform.position = GetTargetPosition();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = GetTargetPosition(); // Smoothly interpolate the camera's position on the x-axis
        float targetPosX = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref _velocity.x, _dampeningX);

        // Smoothly interpolate the camera's position on the y-axis
        float targetPosY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref _velocity.y, _dampeningY);



        // Update the camera's position
        transform.position = new Vector3(targetPosX, targetPosY, transform.position.z);

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
