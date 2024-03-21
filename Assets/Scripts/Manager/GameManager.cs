using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonHandler<GameManager>
{

    /// <summary>Ŀ���� ����ֳ� �ƴѰ� Ȯ���ϴ� �Լ�</summary>
    private bool _isCursorLocked;
    public bool IsCursorLocked => _isCursorLocked;


    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }


    /// <summary>Ŀ���� ��״� �Լ�</summary>
    public void LockCursor()
    {
        _isCursorLocked = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    /// <summary>Ŀ�� ����� Ǫ�� �Լ�</summary>
    public void UnLockCursor()
    {
        _isCursorLocked =  false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
