using System;
using UnityEngine;

public interface ISoundEffect
{
    public AudioSource AudioSource { get; }

    public float TmpVolume { get; set; }


    public virtual void OnVolumeChanged(float value)
    {      
        AudioSource.volume = TmpVolume * value;
    }


    public virtual void Init()
    {
        TmpVolume = AudioSource.volume;
        AudioSource.volume = TmpVolume * SoundManager.Instance.EffectVolumeMul;
        SoundManager.Instance.OnEffectVolumeChanged += OnVolumeChanged;
    }


    public virtual void Destory()
    {
        SoundManager.Instance.OnEffectVolumeChanged -= OnVolumeChanged;
    }

}
