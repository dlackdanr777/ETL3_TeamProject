using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonHandler<GameManager>
{

    /// <summary>커서가 잠겨있나 아닌가 확인하는 함수</summary>
    private bool _isCursorLocked;
    public bool IsCursorLocked => _isCursorLocked;


    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }


    /// <summary>커서를 잠그는 함수</summary>
    public void LockCursor()
    {
        _isCursorLocked = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    /// <summary>커서 잠금을 푸는 함수</summary>
    public void UnLockCursor()
    {
        _isCursorLocked =  false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
