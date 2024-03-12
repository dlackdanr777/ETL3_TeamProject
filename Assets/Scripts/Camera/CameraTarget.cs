using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 _followOffset;

    private Vector3 _currentVelocity = new Vector3(10, 10, 10);
    void Update()
    {
        Vector3 targetPos = target.position + _followOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _currentVelocity, 0.2f);

    }
}
