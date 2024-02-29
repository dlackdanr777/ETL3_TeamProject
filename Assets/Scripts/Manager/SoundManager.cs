using Muks.Tween;
using System;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    Master,
    BackgroundAudio,
    EffectAudio,
    Count,
}

public enum SoundEffectType
{
    ButtonClick,
    MenuOpen,
    MenuClose,
    Count
}


public class SoundManager : SingletonHandler<SoundManager>
{
    [Header("Scripts")]
    [SerializeField] AudioMixer _audioMixer;

    [Header("Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float _backgroundVolume;

    [Range(0f, 1f)]
    [SerializeField] private float _effectVolume;

    [Space]
    [Header("Sounds")]
    [SerializeField] private AudioClip _buttonClickSound;

    [SerializeField] private AudioClip _menuOpenSound;

    [SerializeField] private AudioClip _menuCloseSound;

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

        for (int i = (int)AudioType.BackgroundAudio, count = (int)AudioType.Count; i < count; i++)
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
        _audios[(int)AudioType.BackgroundAudio].outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master")[1];

        _audios[(int)AudioType.EffectAudio].loop = false;
        _audios[(int)AudioType.EffectAudio].playOnAwake = false;
        _audios[((int)AudioType.EffectAudio)].volume = _effectVolume;
        _audios[(int)AudioType.EffectAudio].outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master")[2];

    }


    public void PlayBackgroundMusic(AudioClip clip)
    {
        _audios[(int)AudioType.BackgroundAudio].clip = clip;
        _audios[(int)AudioType.BackgroundAudio].Play();
    }


    public void PlayEffectSound(SoundEffectType type)
    {
        switch (type)
        {
            case SoundEffectType.ButtonClick:
                _audios[(int)AudioType.EffectAudio].PlayOneShot(_buttonClickSound);
                break;
            case SoundEffectType.MenuOpen:
                _audios[(int)AudioType.EffectAudio].PlayOneShot(_menuOpenSound);
                break;
            case SoundEffectType.MenuClose:
                _audios[(int)AudioType.EffectAudio].PlayOneShot(_menuCloseSound);
                break;
        }
    }


    public void PlayEffectSound(AudioClip clip)
    {
        _audios[(int)AudioType.EffectAudio].PlayOneShot(clip);
    }


    public void SetVolume(float value, AudioType type)
    {
        float volume = value != 0 ? Mathf.Log10(value) * 20 : -80;

        switch (type)
        {
            case AudioType.Master:
                _audioMixer.SetFloat("Master", volume);
                break;

            case AudioType.BackgroundAudio:
                _audioMixer.SetFloat("BackgroundMusic", volume);
                break;

            case AudioType.EffectAudio:
                _audioMixer.SetFloat("SoundEffect", volume);
                break;
        }
    }
}

