using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelScene : MonoBehaviour
{
    [Space]
    [Header("Player UI")]
    [SerializeField] private Character _player;
    [SerializeField] private UIPlayer _uiPlayerPrefab;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        UIPlayer uiPlayer = Instantiate(_uiPlayerPrefab);
        uiPlayer.Init(_player);
    }
}
