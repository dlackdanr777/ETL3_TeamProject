using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonHandler<UIManager>
{
    public GameObject startMenu;
    public InputField usernameField;

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Clinet.Instance.ConnectToServer();
    }
}
