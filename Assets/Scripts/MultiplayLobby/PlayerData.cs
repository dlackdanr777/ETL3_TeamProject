using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _nameText;

    public void Init(string name)
    {
        _nameText.text = name;
    }
}
