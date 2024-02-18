using Muks.PCUI;
using Muks.Tween;
using UnityEngine;
using UnityEngine.UI;

public enum MainMenuUiName
{
    UITitle,
    UIHome
}

[RequireComponent(typeof(UINavigation))]
public class UIMainMenu : MonoBehaviour
{
    [Space]
    [Header("Sword Animation")]
    [SerializeField] private Image _swordImage;

    [Range(0f, 1f)]
    [Tooltip("�� �ִϸ��̼��� ��ǥ ���� ��")]
    [SerializeField] private float _swordTargetAlpha;

    [Range(0.1f, 5f)]
    [Tooltip("�� �ִϸ��̼� �ð�")]
    [SerializeField] private float _swordTargetTime;

    [SerializeField] private TweenMode _swordTweenMode;

    [Space]
    [SerializeField] private GameObject _dontTouchArea;
    public GameObject DontTouchArea => _dontTouchArea;

    private UINavigation _uiNav;


    public void Init()
    {
        _uiNav = GetComponent<UINavigation>();
        Invoke("ShowTitle", 0.002f);
        StartMainMenu();
    }


    private void ShowTitle()
    {
        _uiNav.Show("UITitle");
    }


    private void StartMainMenu()
    {
        Tween.IamgeAlpha(_swordImage.gameObject, _swordTargetAlpha, _swordTargetTime, _swordTweenMode).Loop(LoopType.Yoyo);
    }

    
}
