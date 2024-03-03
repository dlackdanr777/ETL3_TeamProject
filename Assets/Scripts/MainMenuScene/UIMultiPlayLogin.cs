using UnityEngine;
using Muks.PCUI;
using System;
using Muks.Tween;
using TMPro;
using UnityEngine.UI;
using Muks.Photon;

[RequireComponent(typeof(CanvasGroup))]
public class UIMultiPlayLogin : UIView
{
    [Header("ShowUI Animation Setting")]
    [SerializeField] private float _startAlpha = 0;
    [SerializeField] private float _targetAlpha = 1;
    [SerializeField] private float _showDuration;
    [SerializeField] private TweenMode _showTweenMode;

    [Space]
    [Header("Components")]
    [SerializeField] private GameObject _unavalkableIdText;
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private Button _connectButton;
    [SerializeField] private Button _exitButton;


    private CanvasGroup _canvasGroup;

    private Vector3 _tmpPos;

    private Vector3 _movePos => new Vector3(0, 10, 0);



    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _canvasGroup = GetComponent<CanvasGroup>();
        _tmpPos = transform.position;

        _unavalkableIdText.gameObject.SetActive(false);

        _exitButton.onClick.AddListener(OnExitButtonClicked);
        _connectButton.onClick.AddListener(OnConnectButtonClicked);
    }


    public override void Hide(Action onCompleted = null)
    {
        VisibleState = VisibleState.Disappearing;

        _nameInputField.text = string.Empty;
        _unavalkableIdText.SetActive(false);

        transform.position = _tmpPos;
        _canvasGroup.alpha = _targetAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.TransformMove(gameObject, _tmpPos - _movePos, _showDuration, _showTweenMode);
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

        transform.position = _tmpPos + _movePos;
        _canvasGroup.alpha = _startAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.TransformMove(gameObject, _tmpPos, _showDuration, _showTweenMode);
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

        InputEsc();
    }


    /// <summary>키입력(Esc)을 감지하여 해당 화면을 나가게 해주는 함수</summary>
    private void InputEsc()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        _uiNav.Hide("UIMultiPlayLogin");
        SoundManager.Instance.PlayEffectSound(SoundEffectType.MenuClose);
    }


    private void OnConnectButtonClicked()
    {
        string nickName = _nameInputField.text;

        //만약 닉네임 칸이 비어있거나 띄어쓰기만 있다면?
        if (string.IsNullOrWhiteSpace(nickName))
        {
            _unavalkableIdText.SetActive(true);
            return;
        }

        _unavalkableIdText.SetActive(false);

        _connectButton.interactable = false;
        PhotonManager.Instance.MasterServerConnect(_nameInputField.text);
    }


    private void OnExitButtonClicked()
    {
        _uiNav.Hide("UIMultiPlayLogin");
        SoundManager.Instance.PlayEffectSound(SoundEffectType.MenuClose);
    }

}
