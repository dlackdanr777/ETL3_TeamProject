using Muks.Tween;
using UnityEngine;
using UnityEngine.UI;

public enum MainMenuUiName
{
    UITitle,
    UIHome
}


public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private UIMainMenuChild _uiTitle;

    [SerializeField] private UIMainMenuChild _uiHome;

    [Space]
    [Header("Sword Animation")]
    [SerializeField] private Image _swordImage;

    [Range(0f, 1f)]
    [Tooltip("검 애니메이션의 목표 알파 값")]
    [SerializeField] private float _swordTargetAlpha;

    [Range(0.1f, 5f)]
    [Tooltip("검 애니메이션 시간")]
    [SerializeField] private float _swordTargetTime;

    [SerializeField] private TweenMode _swordTweenMode;

    [Space]
    [SerializeField] private GameObject _dontTouchArea;
    public GameObject DontTouchArea => _dontTouchArea;


    public void Init()
    {
        _uiTitle.Init(this);
        _uiHome.Init(this);
        StartMainMenu();
    }


    public void ChangeUI(MainMenuUiName uiName)
    {
        switch (uiName)
        {
            case MainMenuUiName.UITitle:
                break;

            case MainMenuUiName.UIHome:
                _uiHome.StartUI();
                break;
        }
    }


    private void StartMainMenu()
    {
        Tween.IamgeAlpha(_swordImage.gameObject, _swordTargetAlpha, _swordTargetTime, _swordTweenMode).Loop(LoopType.Yoyo);
    }

    
}
