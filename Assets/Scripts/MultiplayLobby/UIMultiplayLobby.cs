using ExitGames.Client.Photon;
using Muks.PCUI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UINavigation))]
public class UIMultiplayLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private UIRoom _uiRoom;

    [SerializeField] private Button _joinRoomButton;

    [SerializeField] private Button _createRoomButton;

    [SerializeField] private GameObject _roomListItemPrefab;

    [SerializeField] private Transform _rtContent;

    [SerializeField] private TextMeshProUGUI _userNameText;

    // �� ����� ������ �ִ� Dictionary ����
    private Dictionary<string, RoomInfo> _dicRoomInfo = new Dictionary<string, RoomInfo>();

    private List<RoomListItem> _roomList = new List<RoomListItem>();

    private UINavigation _uiNav;

    private string _selectRoomName;


    void Start()
    {
        _joinRoomButton.onClick.AddListener(OnClickJoinRoom);
        _createRoomButton.onClick.AddListener(OnCreateRoomButtonClicked);
        _userNameText.text = PhotonNetwork.NickName;

        _uiNav = GetComponent<UINavigation>();
        _uiRoom.Init();
    }


    //�� ����� ��ȭ�� ���� �� ȣ��Ǵ� �Լ�
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("���� ��ȭ");
        base.OnRoomListUpdate(roomList);

        _selectRoomName = string.Empty;

        //Content�� �ڽ����� �پ��ִ� Item�� �� ����
        DeleteRoomListItem();
        //dicRoomInfo ������ roomList�� �̿��ؼ� ����
        UpdateRoomListItem(roomList);
        //dicRoom�� ������� roomListItem�� ������
        CreateRoomListItem();

    }


    void SelectRoomItem(string roomName)
    {
        _selectRoomName = roomName;

        for(int i = 0, count = _roomList.Count; i < count; i++)
        {
            _roomList[i].Unselected();
        }
    }


    void DeleteRoomListItem()
    {
        _roomList.Clear();
        foreach (Transform tr in _rtContent)
        {
            Destroy(tr.gameObject);
        }
    }


    void UpdateRoomListItem(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            Debug.Log(info.PlayerCount);
            if (info.PlayerCount <= 0)
            {

                if(_dicRoomInfo.ContainsKey(info.Name))
                    _dicRoomInfo.Remove(info.Name); //����

                continue;
            }
                
            //dicRoomInfo�� info �� ���̸����� �Ǿ��ִ� key���� �����ϴ°�
            if (_dicRoomInfo.ContainsKey(info.Name))
            {
                //���࿡ ���� �����Ǿ�����?
                if (info.RemovedFromList)
                {
                    _dicRoomInfo.Remove(info.Name); //����
                    continue;
                }
            }
            _dicRoomInfo[info.Name] = info; //�߰�
        }
    }


    void CreateRoomListItem()
    {
        foreach (RoomInfo info in _dicRoomInfo.Values)
        {
            //�� ���� ������ ���ÿ� ScrollView-> Content�� �ڽ����� ����
            GameObject go = Instantiate(_roomListItemPrefab, _rtContent);
            //������ item���� RoomListItem ������Ʈ�� �����´�.
            RoomListItem item = go.GetComponent<RoomListItem>();
            //������ ������Ʈ�� ������ �ִ� SetInfo �Լ� ����
            item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //item Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ� ���
            _roomList.Add(item);
            item.OnClickHandler = SelectRoomItem;
        }
    }


    public void OnClickJoinRoom()
    {
        // �� ����
        if (string.IsNullOrWhiteSpace(_selectRoomName))
        {
            Debug.Log("������ ���� �����ϴ�.");
            return;
        }

        PhotonNetwork.JoinRoom(_selectRoomName);
    }


    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("�� ���� ����");
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("�� ���� ����");
        _uiRoom.Show();
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("�� ���� ����" + message);
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("�� ���� ����" + message);
    }


    private void OnCreateRoomButtonClicked()
    {
        SoundManager.Instance.PlayEffectSound(SoundEffectType.ButtonClick);
        _uiNav.Show("UICreateRoom");
    }

}
