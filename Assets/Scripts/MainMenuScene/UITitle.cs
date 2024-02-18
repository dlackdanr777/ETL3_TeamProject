using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using Muks.PCUI;

public class UITitle : UIView
{
    [Header("Button Animation")]
    [SerializeField] private TextMeshProUGUI _pressAniButton;

    [Range(0f, 1f)]
    [Tooltip("�ϴ� �Ʒ� �ؽ�Ʈ �ִϸ��̼��� ��ǥ ���� ��")]
    [SerializeField] private float _aniButtonTargetAlpha;

    [Range(0.1f, 5f)]
    [Tooltip("�ϴ� �Ʒ� �ؽ�Ʈ �ִϸ��̼� �ð�")]
    [SerializeField] private float _aniButtonTargetTime;

    [SerializeField] private TweenMode _aniButtonTweenMode;

    [Space]
    [SerializeField] private Button _backgroundButton;

    [Tooltip("UI���� �ִϸ��̼� �ð�")]
    [SerializeField] private float _changeUiTime;

    private bool _isButtonClicked;
    

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);

        _backgroundButton.onClick.AddListener(() => Hide(() => _uiNav.Show("UIHome")));
    }


    private void Update()
    {
        if (Input.anyKeyDown)
            Hide(() => _uiNav.Show("UIHome"));
    }


    public override void Show(Action onCompleted = null)
    {
        gameObject.SetActive(true);
        VisibleState = VisibleState.Appeared;

        Tween.TMPAlpha(_pressAniButton.gameObject, _aniButtonTargetAlpha, _aniButtonTargetTime, _aniButtonTweenMode, onCompleted).Loop(LoopType.Yoyo);
    }


    public override void Hide(Action onCompleted = null)
    {
        if (_isButtonClicked)
            return;

        _isButtonClicked = true;
        VisibleState = VisibleState.Disappearing;
        _pressAniButton.color = new Color(_pressAniButton.color.r, _pressAniButton.color.g, _pressAniButton.color.b, 1);

        Tween.Stop(_pressAniButton.gameObject);
        Tween.TMPAlpha(_pressAniButton.gameObject, 0, _changeUiTime, TweenMode.Constant, () =>
        {
            VisibleState = VisibleState.Disappeared;
            gameObject.SetActive(false);
            onCompleted?.Invoke();
        });
    }
}
