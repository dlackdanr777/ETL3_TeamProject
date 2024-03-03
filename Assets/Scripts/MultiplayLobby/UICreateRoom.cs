using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.PCUI;
using System;
using Muks.Tween;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UICreateRoom : UIView
{
    [Header("ShowUI Animation Setting")]
    [SerializeField] private float _startAlpha = 0;
    [SerializeField] private float _targetAlpha = 1;
    [SerializeField] private float _showDuration;
    [SerializeField] private TweenMode _showTweenMode;

    [Space]
    [Header("Components")]
    [SerializeField] private TMP_InputField _roomNameInput;
    [SerializeField] private TMP_InputField _maxPlayerInput;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _exitButton;

    private CanvasGroup _canvasGroup;

    private Vector3 _tmpPos;

    private Vector3 _movePos => new Vector3(0, 10, 0);

    private bool _buttonInteractabledToRoomName;
    private bool _buttonInteractabledToMaxPlayer;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _canvasGroup = GetComponent<CanvasGroup>();
        _tmpPos = _rectTransform.anchoredPosition;

        _roomNameInput.onValueChanged.AddListener(OnRoomNameChanged);
        _maxPlayerInput.onValueChanged.AddListener(OnMaxPlayChanged);

        _createRoomButton.onClick.AddListener(CreateRoomButtonClicked);
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

        _buttonInteractabledToRoomName = false;
        _buttonInteractabledToMaxPlayer = false;
        _createRoomButton.interactable = false;

        _roomNameInput.text = string.Empty;
        _maxPlayerInput.text = string.Empty;

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
        UIView view = _uiNav.GetTopView();
        if (view == null || view != this)
            return;

        InputEsc();
    }


    /// <summary>Ű�Է�(Esc)�� �����Ͽ� �ش� ȭ���� ������ ���ִ� �Լ�</summary>
    private void InputEsc()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        _uiNav.Hide("UICreateRoom");
        SoundManager.Instance.PlayEffectSound(SoundEffectType.MenuClose);
    }


    // ���� ��ư Ŭ���� ȣ��Ǵ� �Լ�
    private void OnClickCreateRoom()
    {
        //�� �ɼ�
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = int.Parse(_maxPlayerInput.text);

        //�� ��Ͽ� ���̰� �Ұ��ΰ�?
        options.IsVisible = true;

        //�濡 ���� ���� ����
        options.IsOpen = true;

        //�� ����
        PhotonNetwork.CreateRoom(_roomNameInput.text, options);
    }


    private void CreateRoomButtonClicked()
    {
        _uiNav.Hide("UICreateRoom");

        //�� �ɼ�
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = int.Parse(_maxPlayerInput.text);

        //�� ��Ͽ� ���̰� �Ұ��ΰ�?
        options.IsVisible = true;

        //�濡 ���� ���� ����
        options.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom(_roomNameInput.text, options, TypedLobby.Default);
    }


    private void OnRoomNameChanged(string roomName)
    {
        _buttonInteractabledToRoomName = string.IsNullOrWhiteSpace(roomName) ? false : true;
        ButtonInteractabledCheck();
    }


    private void OnMaxPlayChanged(string maxPlayer)
    {
        if (int.TryParse(maxPlayer, out int maxPlayerToInt))
        {
            if (1 <= maxPlayerToInt && maxPlayerToInt <= 4)
            {
                _buttonInteractabledToMaxPlayer = true;
                ButtonInteractabledCheck();
                return;
            }
        }

        _buttonInteractabledToMaxPlayer = false;
        ButtonInteractabledCheck();
    }


    private void ButtonInteractabledCheck()
    {
        _createRoomButton.interactable = _buttonInteractabledToMaxPlayer && _buttonInteractabledToRoomName ? true : false;
    }


    private void OnExitButtonClicked()
    {
        _uiNav.Hide("UICreateRoom");
    }

}
