using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : UIMainMenuChild
{
    [SerializeField] private UIHomeMenu[] _homeMenus;

    [SerializeField] private Image _selectEffect;

    private int _homeMenusCount;

    private int _currentMenuIndex;

    private bool _inputEnabled = true;


    public override void Init(UIMainMenu uiMainMenu)
    {
        base.Init(uiMainMenu);
        _uiMainMenu = uiMainMenu;
        _homeMenusCount = _homeMenus.Length;
        _currentMenuIndex = 0;

        RefreshSelectMenu();

        for(int i = 0; i < _homeMenusCount; i++)
        {
            int menuIndex = i;
            _homeMenus[i].Init(() => SelectMenu(menuIndex));
        }

        gameObject.SetActive(false);
    }


    public override void StartUI()
    {
        gameObject.SetActive(true);
        _uiMainMenu.DontTouchArea.SetActive(false);
    }


    protected override void ExitUI()
    {
        throw new System.NotImplementedException();
    }


    private void Update()
    {
        InputVertical();
        InputEnter();
    }


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

        //음수 일 경우 Count - 현재 인덱스
        // Count 범위를 넘어갈 경우 나머지를 계산해서 대입
        _currentMenuIndex = _currentMenuIndex < 0
            ? _currentMenuIndex + _homeMenusCount
            : _currentMenuIndex % _homeMenusCount;

        RefreshSelectMenu();
    }

    
    private void InputEnter()
    {
        if (!Input.GetKeyDown(KeyCode.Return))
            return;
            
        SelectMenu(_currentMenuIndex);
    }


    private void SelectMenu(int menuIndex)
    {
        Debug.Log(menuIndex);
        if(_currentMenuIndex != menuIndex)
        {
            _currentMenuIndex = menuIndex;
            RefreshSelectMenu();
        }

        else
        {
            Debug.Log("UI이동");
        }

    }


    private void RefreshSelectMenu()
    {
        _selectEffect.transform.position = _homeMenus[_currentMenuIndex].transform.position + new Vector3(0, 9, 0) ;
    }
}
