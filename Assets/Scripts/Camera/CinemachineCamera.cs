using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CinemachineCamera : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera _mainVitualCamera;
    [SerializeField] private GameObject _rotateTarget;

    [Space]
    [Header("Cameras")]
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _shakeAmplitude = 1.2f;
    [SerializeField] private float _shakeFrequency = 2.0f;


    private CinemachineBasicMultiChannelPerlin _virtualCameraNoise;
    private float _shakeTime;
    private float MouseY;
    private float MouseX;

    private void Awake()
    {
        if (_virtualCameraNoise == null)
            _virtualCameraNoise = _mainVitualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    private void Update()
    {
        CameraControll();
        UpdateCameraShake();
    }


    protected virtual void CameraControll()
    {
        MouseX += Input.GetAxisRaw("Mouse X") * _rotateSpeed * Time.deltaTime;

        MouseY -= Input.GetAxisRaw("Mouse Y") * _rotateSpeed * Time.deltaTime;

        MouseY = Mathf.Clamp(MouseY, -70f, 70f);

        _rotateTarget.transform.localRotation = Quaternion.Euler(MouseY, MouseX, 0f);
    } 


    /// <summary> 카메라 흔들림 수치를 설정 하는 함수</summary> 
    public void CameraShake(float duration, float shakeAmplitude, float shakeFrequency)
    {
        _shakeTime = duration;
        _virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
        _virtualCameraNoise.m_FrequencyGain = shakeFrequency;
    }

    /// <summary> 카메라 흔들림 수치를 설정 하는 함수</summary> 
    public void CameraShake(float duration)
    {
        _shakeTime = duration;
        _virtualCameraNoise.m_AmplitudeGain = _shakeAmplitude;
        _virtualCameraNoise.m_FrequencyGain = _shakeFrequency;
    }

    public void StopShake()
    {
        _shakeTime = 0;
    }


    /// <summary> 카메라에 흔들림을 주는 함수 </summary> 
    private void UpdateCameraShake()
    {
        if (_virtualCameraNoise != null)
        {
            if (_shakeTime > 0)
            {
                _shakeTime -= Time.deltaTime;
            }
            else
            {
                _virtualCameraNoise.m_AmplitudeGain = 0f;
                _shakeTime = 0f;
            }
        }
    }
}
