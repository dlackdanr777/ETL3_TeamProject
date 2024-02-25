using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>MainMenu에서 관리하는 UI의 부모 클래스</summary>
public abstract class UIMainMenuChild : MonoBehaviour
{
    protected UIMainMenu _uiMainMenu;

    public virtual void Init(UIMainMenu uiMainMenu)
    {
        _uiMainMenu = uiMainMenu;
    }

    /// <summary>현재 UI를 시작하는 함수</summary>
    public abstract void StartUI();

    /// <summary>현재 UI를 끝내는 함수</summary>
    protected abstract void ExitUI();

}
