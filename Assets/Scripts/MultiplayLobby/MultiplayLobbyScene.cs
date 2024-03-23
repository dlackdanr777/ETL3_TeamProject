using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayLobbyScene : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundMusic;
    void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic(_backgroundMusic, 1f);
    }

}
