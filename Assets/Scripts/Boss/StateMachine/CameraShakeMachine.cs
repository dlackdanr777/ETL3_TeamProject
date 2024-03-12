using System;
using UnityEngine;

[Serializable]
public class CameraShakeData
{
    [Header("카메라 흔들림 지속 시간")]
    [SerializeField] private float _duration = 0.5f;
    public float Duration => _duration;

    [Header("카메라 흔들림 진폭")]
    [SerializeField] private float _amplitude = 1f;
    public float Amplitude => _amplitude;

    [Header("카메라 흔들림 빈도")]
    [SerializeField] private float _frequency = 0.5f;
    public float Frequency => _frequency;


    [Tooltip("활성화 프레임")]
    [SerializeField] private int _startFrame;
    public int StartFrame => _startFrame;

    [Tooltip("비 활성화 프레임")]
    [SerializeField] private int _finishedFrame;
    public int FinishedFrame => _finishedFrame;


    private bool _isStarted;
    public bool GetIsStarted => _isStarted;
    public bool SetIsStarted { set { _isStarted = value; } }

    private bool _isFinished;
    public bool GetIsFinished => _isFinished;
    public bool SetIsFinished { set { _isFinished = value; } }
}

public class CameraShakeMachine : StateMachineBehaviour
{

    [SerializeField] private CameraShakeData[] _shakeData;

    private AnimationClip _clip;

    private CinemachineCamera[] _cameras;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0, count = _shakeData.Length; i < count; i++)
        {
            _shakeData[i].SetIsStarted = false;
            _shakeData[i].SetIsFinished = false;
        }
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        float currentTime = _clip.length * stateInfo.normalizedTime;
        int _currentFrame = Mathf.RoundToInt(_clip.frameRate * currentTime);

        for (int i = 0, count = _shakeData.Length; i < count; i++)
        {
            if (_shakeData[i].StartFrame <= _currentFrame && !_shakeData[i].GetIsStarted)
            {
                _cameras = FindObjectsOfType<CinemachineCamera>();
                Debug.Log(_cameras.Length);
                for(int j = 0, countJ = _cameras.Length; j < countJ; j++)
                {
                    _cameras[j].CameraShake(_shakeData[i].Duration, _shakeData[i].Amplitude, _shakeData[i].Frequency);
                }

                _shakeData[i].SetIsStarted = true;
            }

            else if (_shakeData[i].FinishedFrame <= _currentFrame && !_shakeData[i].GetIsFinished)
            {
                _cameras = FindObjectsOfType<CinemachineCamera>();
                for (int j = 0, countJ = _cameras.Length; j < countJ; j++)
                {
                    _cameras[j].StopShake();
                }
                _shakeData[i].SetIsFinished = true;
            }
        }
    }
}
