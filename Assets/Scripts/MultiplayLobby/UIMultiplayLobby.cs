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

    // 방 목록을 가지고 있는 Dictionary 변수
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


    //방 목록의 변화가 있을 때 호출되는 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("방목록 변화");
        base.OnRoomListUpdate(roomList);

        _selectRoomName = string.Empty;

        //Content에 자식으로 붙어있는 Item을 다 삭제
        DeleteRoomListItem();
        //dicRoomInfo 변수를 roomList를 이용해서 갱신
        UpdateRoomListItem(roomList);
        //dicRoom을 기반으로 roomListItem을 만들자
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
                    _dicRoomInfo.Remove(info.Name); //삭제

                continue;
            }
                
            //dicRoomInfo에 info 의 방이름으로 되어있는 key값이 존재하는가
            if (_dicRoomInfo.ContainsKey(info.Name))
            {
                //만약에 방이 삭제되었으면?
                if (info.RemovedFromList)
                {
                    _dicRoomInfo.Remove(info.Name); //삭제
                    continue;
                }
            }
            _dicRoomInfo[info.Name] = info; //추가
        }
    }


    void CreateRoomListItem()
    {
        foreach (RoomInfo info in _dicRoomInfo.Values)
        {
            //방 정보 생성과 동시에 ScrollView-> Content의 자식으로 하자
            GameObject go = Instantiate(_roomListItemPrefab, _rtContent);
            //생성된 item에서 RoomListItem 컴포넌트를 가져온다.
            RoomListItem item = go.GetComponent<RoomListItem>();
            //가져온 컴포넌트가 가지고 있는 SetInfo 함수 실행
            item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //item 클릭되었을 때 호출되는 함수 등록
            _roomList.Add(item);
            item.OnClickHandler = SelectRoomItem;
        }
    }


    public void OnClickJoinRoom()
    {
        // 방 참여
        if (string.IsNullOrWhiteSpace(_selectRoomName))
        {
            Debug.Log("선택한 룸이 없습니다.");
            return;
        }

        PhotonNetwork.JoinRoom(_selectRoomName);
    }


    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("방 생성 성공");
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("방 입장 성공");
        _uiRoom.Show();
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("방 입장 실패" + message);
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("방 생성 실패" + message);
    }


    private void OnCreateRoomButtonClicked()
    {
        SoundManager.Instance.PlayEffectSound(SoundEffectType.ButtonClick);
        _uiNav.Show("UICreateRoom");
    }

}
