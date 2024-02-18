using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIHomeMenuButton : MonoBehaviour
{
    [SerializeField] private Button _button;


    public void Init(UnityAction onClicked)
    {
        _button.onClick.AddListener(onClicked);
    }
}
