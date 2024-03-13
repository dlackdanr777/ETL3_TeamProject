using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{

    [SerializeField] private Vector3 _followOffset;
    [SerializeField] private Transform _target;

    private Vector3 _currentVelocity = new Vector3(10, 10, 10);

    void Update()
    {
        if (_target == null)
            return;

        Vector3 targetPos = _target.position + _followOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _currentVelocity, 0.2f);
    }

    public void Init(Transform target)
    {
        _target = target;
    }
}
