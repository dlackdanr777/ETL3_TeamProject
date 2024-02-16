using TMPro;
using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;

public class UITitle : UIMainMenuChild
{
    [Header("Button Animation")]
    [SerializeField] private TextMeshProUGUI _pressAniButton;

    [Range(0f, 1f)]
    [Tooltip("하단 아래 텍스트 애니메이션의 목표 알파 값")]
    [SerializeField] private float _aniButtonTargetAlpha;

    [Range(0.1f, 5f)]
    [Tooltip("하단 아래 텍스트 애니메이션 시간")]
    [SerializeField] private float _aniButtonTargetTime;

    [SerializeField] private TweenMode _aniButtonTweenMode;

    [Space]
    [SerializeField] private Button _backgroundButton;

    [Tooltip("UI변경 애니메이션 시간")]
    [SerializeField] private float _changeUiTime;

    private bool _isButtonClicked;
    

    public override void Init(UIMainMenu uiMainMenu)
    {
        base.Init(uiMainMenu);
        _backgroundButton.onClick.AddListener(ExitUI);

        StartUI();
    }


    private void Update()
    {
        if (Input.anyKeyDown)
            ExitUI();
    }


    public override void StartUI()
    {
        Tween.TMPAlpha(_pressAniButton.gameObject, _aniButtonTargetAlpha, _aniButtonTargetTime, _aniButtonTweenMode).Loop(LoopType.Yoyo);
    }


    protected override void ExitUI()
    {
        if (_isButtonClicked)
            return;

        _isButtonClicked = true;
        _pressAniButton.color = new Color(_pressAniButton.color.r, _pressAniButton.color.g, _pressAniButton.color.b, 1);

        Tween.Stop(_pressAniButton.gameObject);
        Tween.TMPAlpha(_pressAniButton.gameObject, 0, _changeUiTime, TweenMode.Constant, () =>
        {
            _uiMainMenu.ChangeUI(MainMenuUiName.UIHome);
            gameObject.SetActive(false);
        });
    }
}
