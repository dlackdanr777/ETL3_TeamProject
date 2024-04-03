using System;
using System.Collections;
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
    [Header("Components")]
    [SerializeField] private  AudioMixer _audioMixer;

    [Header("Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float _backgroundVolume;

    [Range(0f, 1f)]
    [SerializeField] private float _effectVolume;

    [Space]
    [Header("AudioClips")]
    [SerializeField] private AudioClip _buttonClickSound;

    [SerializeField] private AudioClip _menuOpenSound;

    [SerializeField] private AudioClip _menuCloseSound;


    private AudioSource[] _audios;

    //배경 음악 변경시 볼륨 업, 다운 기능을 위한 변수
    private Coroutine _changeAudioRoutine;
    private Coroutine _stopBackgroundAudioRoutine;

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

        _audios[(int)AudioType.BackgroundAudio].loop = true;
        _audios[(int)AudioType.BackgroundAudio].playOnAwake = true;
        _audios[(int)AudioType.BackgroundAudio].volume = _backgroundVolume;
        _audios[(int)AudioType.BackgroundAudio].outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master")[1];

        _audios[(int)AudioType.EffectAudio].loop = false;
        _audios[(int)AudioType.EffectAudio].playOnAwake = false;
        _audios[((int)AudioType.EffectAudio)].volume = _effectVolume;
        _audios[(int)AudioType.EffectAudio].outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master")[2];

    }


    public void PlayBackgroundMusic(AudioClip clip, float duration = 1, bool isLoop = true)
    {
        if (_changeAudioRoutine != null)
            StopCoroutine(_changeAudioRoutine);

        _audios[(int)AudioType.BackgroundAudio].loop = isLoop;
        _audios[(int)AudioType.BackgroundAudio].Play();
        if (duration == 0)
        {
            _audios[(int)AudioType.BackgroundAudio].volume = _backgroundVolume;
            _audios[(int)AudioType.BackgroundAudio].clip = clip;
            return;
        }

        _changeAudioRoutine = StartCoroutine(IEChangeBackgroundAudio(clip, duration));
    }


    public void PlayEffectSound(AudioClip clip)
    {
        _audios[(int)AudioType.EffectAudio].volume = _effectVolume;
        _audios[(int)AudioType.EffectAudio].PlayOneShot(clip);
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


    public void StopBackgroundAudio(float duration = 0)
    {
        if (_stopBackgroundAudioRoutine != null)
            StopCoroutine(_stopBackgroundAudioRoutine);

        if (duration == 0)
        {
            _audios[(int)AudioType.BackgroundAudio].Stop();
            return;
        }

        _stopBackgroundAudioRoutine = StartCoroutine(IEStopBackgroundAudio(duration));
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


    private IEnumerator IEStopBackgroundAudio(float duration)
    {
        float maxVolume = _audios[(int)AudioType.BackgroundAudio].volume;
        float changeDuration = duration;
        float timer = 0;

        while (timer < changeDuration)
        {
            timer += 0.02f;
            _audios[(int)AudioType.BackgroundAudio].volume = Mathf.Lerp(maxVolume, 0, timer / changeDuration);

            yield return YieldCache.WaitForSeconds(0.02f);
        }

        _audios[(int)AudioType.BackgroundAudio].Stop();
    }


    private IEnumerator IEChangeBackgroundAudio(AudioClip clip, float duration)
    {
        float maxVolume = _backgroundVolume;
        float changeDuration = duration * 0.5f;
        float timer = 0;

        while (timer < changeDuration)
        {
            timer += 0.02f;
            _audios[(int)AudioType.BackgroundAudio].volume = Mathf.Lerp(maxVolume, 0, timer / changeDuration);
            yield return YieldCache.WaitForSeconds(0.02f);
        }

        _audios[(int)AudioType.BackgroundAudio].clip = clip;
        _audios[(int)AudioType.BackgroundAudio].volume = 0;
        _audios[(int)AudioType.BackgroundAudio].Play();

        timer = 0;
        while (timer < changeDuration)
        {
            timer += 0.02f;
            _audios[(int)AudioType.BackgroundAudio].volume = Mathf.Lerp(0, maxVolume, timer / changeDuration);

            yield return YieldCache.WaitForSeconds(0.02f);
        }
    }

}

