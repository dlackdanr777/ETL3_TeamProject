using Muks.PCUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : UIView
{
    [SerializeField] private UIButton[] _homeMenus;

    [SerializeField] private Image _selectEffect;

    private List<Action> _selectMenuActionList;

    private int _homeMenusCount;

    private int _currentMenuIndex;

    private bool _inputEnabled = true;


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _homeMenusCount = _homeMenus.Length;
        _currentMenuIndex = 0;

        for(int i = 0; i < _homeMenusCount; i++)
        {
            int menuIndex = i;
            _homeMenus[i].Init(() => SelectMenu(menuIndex));
        }

        _selectMenuActionList = new List<Action>
        {
            StartGameButtonClicked,
            MultiplayButtonClicked,
            SettingsButtonClicked,
            QuitButtonClicked,
        };

        gameObject.SetActive(false);
    }


    public override void Show(Action onCompleted = null)
    {
        VisibleState = VisibleState.Appeared;
        RefreshSelectMenu();
        gameObject.SetActive(true);
    }


    public override void Hide(Action onCompleted = null)
    {
        VisibleState = VisibleState.Disappeared;
    }


    private void Update()
    {
        if (!CheckVisibleState())
            return;

        if (_uiNav.GetTopView() != this)
            return;

        InputVertical();
        InputEnter();
    }


    /// <summary>Ű�Է�(���Ʒ� ȭ��ǥ)�� �����Ͽ� �޴��� ��������ִ� �Լ�</summary>
    private void InputVertical()
    {
        int verticalInput = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        if (verticalInput == 0)
        {
            _inputEnabled = true;
            return;
        }

        if (!_inputEnabled)
            return;

        _inputEnabled = false;

        _currentMenuIndex -= verticalInput;

        //���� �� ��� Count - ���� �ε���
        // Count ������ �Ѿ ��� �������� ����ؼ� ����
        _currentMenuIndex = _currentMenuIndex < 0
            ? _currentMenuIndex + _homeMenusCount
            : _currentMenuIndex % _homeMenusCount;

        SoundManager.Instance.PlayEffectSound(SoundEffectType.ButtonClick);
        RefreshSelectMenu();
    }


    /// <summary>Ű�Է�(����)�� �����Ͽ� ���õ� �޴��� ��������ִ� �Լ�</summary>
    private void InputEnter()
    {
        if (!Input.GetKeyDown(KeyCode.Return))
            return;
            
        SelectMenu(_currentMenuIndex);
    }


    /// <summary>Index�� �޾� �ش� Index��ȣ�� ���� �޴��� �����ϴ� �Լ�</summary>
    private void SelectMenu(int menuIndex)
    {
        if (!CheckVisibleState())
            return;

        if(_currentMenuIndex != menuIndex)
        {
            _currentMenuIndex = menuIndex;
            RefreshSelectMenu();
        }

        else
        {
            _selectMenuActionList[_currentMenuIndex]?.Invoke();
        }

        SoundManager.Instance.PlayEffectSound(SoundEffectType.ButtonClick);
    }


    private void StartGameButtonClicked()
    {
        SoundManager.Instance.PlayEffectSound(SoundEffectType.MenuOpen);
        LoadingSceneManager.LoadScene("BossLevel");
    }


    private void MultiplayButtonClicked()
    {
        _uiNav.Show("UIMultiPlayLogin");
        SoundManager.Instance.PlayEffectSound(SoundEffectType.MenuOpen);
    }


    private void SettingsButtonClicked()
    {
        _uiNav.Show("UISettings");
        SoundManager.Instance.PlayEffectSound(SoundEffectType.MenuOpen);
    }


    private void QuitButtonClicked()
    {
        Application.Quit();
    }


    private bool CheckVisibleState()
    {
        if (VisibleState == VisibleState.Disappearing || VisibleState == VisibleState.Appearing)
        {
            Debug.Log("UI�� �����ų� ������ �� �Դϴ�.");
            return false;
        }

        return true;
    }


    private void RefreshSelectMenu()
    {
        _selectEffect.transform.position = _homeMenus[_currentMenuIndex].transform.position + new Vector3(0, 9, 0) ;
    }


}
