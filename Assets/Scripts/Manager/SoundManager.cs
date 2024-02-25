using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum AudioType
{
    BackgroundAudio,
    EffectAudio,
    Count,
}

public class SoundManager : SingletonHandler<SoundManager>
{
    [Range(0f, 1f)]
    [SerializeField] private float _backgroundVolume;

    [Range(0f, 1f)]
    [SerializeField] private float _effectVolume;

    private AudioSource[] _audios;

    private float _backgroundVolumeMul;
    public float BackgroundVolumeMul => _backgroundVolumeMul;

    private float _effectVolumeMul;
    public float EffectVolumeMul => _effectVolumeMul;   

    public event Action<float> OnEffectVolumeChanged; 

    public override void Awake()
    {
        base.Awake();
        Init();
    }


    private void Init()
    {
        _audios = new AudioSource[(int)AudioType.Count];

        for (int i = 0, count = (int)AudioType.Count; i < count; i++)
        {
            GameObject obj = new GameObject(Enum.GetName(typeof(AudioType), i));
            obj.transform.parent = transform;
            _audios[i] = obj.AddComponent<AudioSource>();
        }

        _backgroundVolumeMul = 1;
        _effectVolumeMul = 1;

        _audios[(int)AudioType.BackgroundAudio].loop = true;
        _audios[(int)AudioType.BackgroundAudio].playOnAwake = true;
        _audios[(int)AudioType.BackgroundAudio].volume = _backgroundVolume;

        _audios[(int)AudioType.EffectAudio].loop = false;
        _audios[(int)AudioType.EffectAudio].playOnAwake = false;
        _audios[((int)AudioType.EffectAudio)].volume = _effectVolume;


    }


    public void Play(AudioClip clip, AudioType type)
    {
        switch (type)
        {
            case AudioType.BackgroundAudio:
                _audios[(int)type].clip = clip;
                _audios[(int)type].Play();
                break;

            case AudioType.EffectAudio:
                _audios[(int)type].PlayOneShot(clip);
                break;
        }
    }


    public void SetVolume(float value, AudioType type)
    {
        if(type == AudioType.BackgroundAudio)
        {
            _backgroundVolumeMul = value;
            _audios[(int)type].volume = _backgroundVolume * value;
        }

        else if (type == AudioType.EffectAudio)
        {
            _effectVolumeMul = value;
            _audios[(int)type].volume = _effectVolume * value;
            OnEffectVolumeChanged?.Invoke(value);
        }
    }
}

