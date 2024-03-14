using UnityEngine;
using Muks.PCUI;
using System;
using Muks.Tween;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UISettings : UIView
{
    [Space]
    [Header("ShowUI Animation Setting")]
    [SerializeField] private float _startAlpha = 0;
    [SerializeField] private float _targetAlpha = 1;
    [SerializeField] private float _showDuration;
    [SerializeField] private TweenMode _showTweenMode;

    [Space]
    [Header("Components")]
    [SerializeField] private RectTransform _selectEffect;
    [SerializeField] private UIButton[] _settingButtons;
    [SerializeField] private TextMeshProUGUI[] _buttonText;
    [SerializeField] private GameObject[] _buttonCanvas;
    [SerializeField] private Button _exitButton;

    private int _buttonsCount;

    private int _currentMenuIndex;

    private CanvasGroup _canvasGroup;

    private Vector3 _tmpPos;

    private Vector3 _movePos => new Vector3(0, 50, 0);

    private bool _inputEnabled = true;


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _canvasGroup = GetComponent<CanvasGroup>();
        _tmpPos = _rectTransform.anchoredPosition;

        _currentMenuIndex = 0;
        _buttonsCount = _settingButtons.Length;
        for (int i = 0; i < _buttonsCount; i++)
        {
            int index = i;
            _settingButtons[i].Init(() => SelectMenu(index));
        }

        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }


    public override void Hide(Action onCompleted = null)
    {
        VisibleState = VisibleState.Disappearing;

        _rectTransform.anchoredPosition = _tmpPos;
        _canvasGroup.alpha = _targetAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.RectTransfromAnchoredPosition(gameObject, _tmpPos - _movePos, _showDuration, _showTweenMode);
        Tween.CanvasGroupAlpha(gameObject, _startAlpha, _showDuration, _showTweenMode, () =>
        {
            VisibleState = VisibleState.Disappeared;
            _canvasGroup.blocksRaycasts = true;

            gameObject.SetActive(false);
            onCompleted?.Invoke();
        });
    }


    public override void Show(Action onCompleted = null)
    {
        VisibleState = VisibleState.Appearing;
        gameObject.SetActive(true);
        SelectMenu(0);

        _rectTransform.anchoredPosition = _tmpPos + _movePos;
        _canvasGroup.alpha = _startAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.RectTransfromAnchoredPosition(gameObject, _tmpPos, _showDuration, _showTweenMode);
        Tween.CanvasGroupAlpha(gameObject, _targetAlpha, _showDuration, _showTweenMode, () =>
        {
            VisibleState = VisibleState.Appeared;
            _canvasGroup.blocksRaycasts = true;
            onCompleted?.Invoke();
        });
    }


    private void Update()
    {
        if (_uiNav.GetTopView() != this)
            return;

        InputVertical();
        InputEsc();
    }



    /// <summary>키입력(Esc)을 감지하여 해당 화면을 나가게 해주는 함수</summary>
    private void InputEsc()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        _uiNav.Hide("UISettings");
        SoundManager.Instance.PlayEffectSound(SoundEffectType.MenuClose);
    }


    private void SelectMenu(int menuIndex)
    {
        for (int i = 0; i < _buttonsCount; i++)
        {
            _buttonCanvas[i].gameObject.SetActive(false);
            _buttonText[i].color = Color.black;
        }

        _buttonCanvas[menuIndex].gameObject.SetActive(true);
        _buttonText[menuIndex].color = Color.white;
        _currentMenuIndex = menuIndex;

        Vector3 effectPos = _settingButtons[menuIndex].RectTransform.anchoredPosition;
        effectPos.x -= _settingButtons[menuIndex].RectTransform.sizeDelta.x * 0.5f;
        _selectEffect.anchoredPosition = effectPos;

        SoundManager.Instance.PlayEffectSound(SoundEffectType.ButtonClick);
    }


    /// <summary>키입력(위아래 화살표)을 감지하여 메뉴를 변경시켜주는 함수</summary>
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
            ? _currentMenuIndex + _buttonsCount
            : _currentMenuIndex % _buttonsCount;

        SelectMenu(_currentMenuIndex);
    }


    private void OnExitButtonClicked()
    {
        _uiNav.Hide("UISettings");
        SoundManager.Instance.PlayEffectSound(SoundEffectType.MenuClose);
    }

}
