using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : MonoBehaviour
{
    [SerializeField] private UIMainMenu _uiMainMenu;

    [Header("Sound")]
    [SerializeField] private AudioClip _backgroundMusic;


    private void Awake()
    {
        _uiMainMenu.Init();

    }

    private void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic(_backgroundMusic);
    }

}
