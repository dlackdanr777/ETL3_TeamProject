using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHome : UIMainMenuChild
{
    public override void Init(UIMainMenu uiMainMenu)
    {
        base.Init(uiMainMenu);
        _uiMainMenu = uiMainMenu;
        gameObject.SetActive(false);
    }


    public override void StartUI()
    {
        gameObject.SetActive(true);
    }


    protected override void ExitUI()
    {
        throw new System.NotImplementedException();
    }
}
