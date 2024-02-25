using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAudioMenu : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;

    [SerializeField] private Slider _gameSoundSlider;

    public void Start()
    {
        Init();
    }


    public void Init()
    {
        _musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        _gameSoundSlider.onValueChanged.AddListener(OnGameSoundSliderValueChanged);
    }


    private void OnMusicSliderValueChanged(float value)
    {
        SoundManager.Instance.SetVolume(value, AudioType.BackgroundAudio);
    }

    
    private void OnGameSoundSliderValueChanged(float value)
    {
        SoundManager.Instance.SetVolume(value, AudioType.EffectAudio);
    }
}
