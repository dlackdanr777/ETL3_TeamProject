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
    [SerializeField] private float _backgroundVolume;

    [SerializeField] private float _effectVolume;

    private AudioSource[] _audios;


    public override void Awake()
    {
        base.Awake();
        Init();
    }


    private void Init()
    {
        for (int i = 0, count = (int)AudioType.Count; i < count; i++)
        {
            GameObject obj = new GameObject(Enum.GetName(typeof(AudioType), i));
            obj.transform.parent = transform;

            _audios[i] = obj.AddComponent<AudioSource>();
        }

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
        _audios[(int)type].volume = value;

        if (type == AudioType.EffectAudio)
        {
            //ISoundEffect.OnVolumeChanged
        }
    }
}
