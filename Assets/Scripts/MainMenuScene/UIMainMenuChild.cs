using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>MainMenu���� �����ϴ� UI�� �θ� Ŭ����</summary>
public abstract class UIMainMenuChild : MonoBehaviour
{
    protected UIMainMenu _uiMainMenu;

    public virtual void Init(UIMainMenu uiMainMenu)
    {
        _uiMainMenu = uiMainMenu;
    }

    /// <summary>���� UI�� �����ϴ� �Լ�</summary>
    public abstract void StartUI();

    /// <summary>���� UI�� ������ �Լ�</summary>
    protected abstract void ExitUI();

}
