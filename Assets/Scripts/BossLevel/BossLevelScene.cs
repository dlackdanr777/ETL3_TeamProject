using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelScene : MonoBehaviour
{
    [Space]
    [Header("Player UI")]
    [SerializeField] private Character _player;
    [SerializeField] private UIPlayer _uiPlayerPrefab;

    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _backgroundAudio;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        UIPlayer uiPlayer = Instantiate(_uiPlayerPrefab);
        uiPlayer.Init(_player);

        SoundManager.Instance.PlayBackgroundMusic(_backgroundAudio);
    }
}
