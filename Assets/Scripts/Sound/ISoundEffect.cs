using System;
using UnityEngine;

public interface ISoundEffect
{
    public static Action<float> OnVolumeChanged;

    public AudioSource AudioSource { get; }

    public float TmpVolume { get; }

}
