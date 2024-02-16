using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : MonoBehaviour
{
    [SerializeField] private UIMainMenu _uiMainMenu;


    private void Awake()
    {
        _uiMainMenu.Init();
    }
}
