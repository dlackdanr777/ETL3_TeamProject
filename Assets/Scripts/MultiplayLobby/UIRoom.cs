using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.PCUI;
using Muks.Tween;
using System;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class UIRoom : MonoBehaviourPunCallbacks
{
    [Header("ShowUI Animation Setting")]
    [SerializeField] private float _startAlpha = 0;
    [SerializeField] private float _targetAlpha = 1;
    [SerializeField] private float _showDuration;
    [SerializeField] private TweenMode _showTweenMode;

    [Space]
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _roomNameText;
    [SerializeField] private TextMeshProUGUI _playerNumText;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Transform _playerLayout;

    [Space]
    [Header("Prefabs")]
    [SerializeField] private PlayerData _playerDataPrefab;

    private CanvasGroup _canvasGroup;

    private RectTransform _rectTransform;

    private Vector3 _tmpPos;
    private Vector3 _movePos => new Vector3(0, 10, 0);

    private List<GameObject> _playerDataList = new List<GameObject>();


    public void Init()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _tmpPos = _rectTransform.anchoredPosition;

        _exitButton.onClick.AddListener(OnExitButtonClicked);
        _startButton.onClick.AddListener(OnStartButtonClicked);

        gameObject.SetActive(false);
    }


    public void Hide(Action onCompleted = null)
    {
        SoundManager.Instance.PlayEffectSound(SoundEffectType.MenuClose);
        _rectTransform.anchoredPosition = _tmpPos;
        _canvasGroup.alpha = _targetAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.RectTransfromAnchoredPosition(gameObject, _tmpPos - _movePos, _showDuration, _showTweenMode);
        Tween.CanvasGroupAlpha(gameObject, _startAlpha, _showDuration, _showTweenMode, () =>
        {
            _canvasGroup.blocksRaycasts = true;

            gameObject.SetActive(false);
            onCompleted?.Invoke();
        });
    }


    public void Show(Action onCompleted = null)
    {
        gameObject.SetActive(true);
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerNum();

        _rectTransform.anchoredPosition = _tmpPos + _movePos;
        _canvasGroup.alpha = _startAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.RectTransfromAnchoredPosition(gameObject, _tmpPos, _showDuration, _showTweenMode);
        Tween.CanvasGroupAlpha(gameObject, _targetAlpha, _showDuration, _showTweenMode, () =>
        {
            PlayerListUpdate();
            _canvasGroup.blocksRaycasts = true;
            onCompleted?.Invoke();
        });
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        PlayerListUpdate();
        Debug.LogFormat("{0}방 입장", newPlayer.NickName);
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PlayerListUpdate();

        Debug.LogFormat("{0}방 퇴장", otherPlayer.NickName);
    }


    private void PlayerListUpdate()
    {
        foreach(GameObject obj in _playerDataList)
        {
            Destroy(obj);
        }
        _playerDataList.Clear();

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            PlayerData playerData = Instantiate(_playerDataPrefab);
            playerData.transform.parent = _playerLayout.transform;
            playerData.Init(player.NickName);

            _playerDataList.Add(playerData.gameObject);
        }

        OnStartButtonEnabled();
        UpdatePlayerNum();
    }


    private void OnStartButtonEnabled()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("방장입니다.");
            _startButton.gameObject.SetActive(true);
            return;
        }

        _startButton.gameObject.SetActive(false);
    }


    private void UpdatePlayerNum()
    {
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        int currentPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        _playerNumText.text = currentPlayer + "/" + maxPlayer;
    }


    private void OnStartButtonClicked()
    {
        _canvasGroup.blocksRaycasts = false;
        PhotonNetwork.LoadLevel("BoosLevel_Multi");
    }


    private void OnExitButtonClicked()
    {
        SoundManager.Instance.PlayEffectSound(SoundEffectType.ButtonClick);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    
        Hide();
    }

}
