using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MultiplayLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _joinRoomButton;
    [SerializeField] private GameObject _roomListItem;
    public Transform rtContent;

    // 방 목록을 가지고 있는 Dictionary 변수
    private Dictionary<string, RoomInfo> _dicRoomInfo = new Dictionary<string, RoomInfo>();

    private string _selectRoomName;


    void Start()
    {
        _joinRoomButton.onClick.AddListener(OnClickJoinRoom);
    }


    //방 목록의 변화가 있을 때 호출되는 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
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
    }


    void DeleteRoomListItem()
    {

        foreach (Transform tr in rtContent)
        {
            Destroy(tr.gameObject);
        }
    }


    void UpdateRoomListItem(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
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
            GameObject go = Instantiate(_roomListItem, rtContent);
            //생성된 item에서 RoomListItem 컴포넌트를 가져온다.
            RoomListItem item = go.GetComponent<RoomListItem>();
            //가져온 컴포넌트가 가지고 있는 SetInfo 함수 실행
            item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //item 클릭되었을 때 호출되는 함수 등록
            item.OnClickHandler = SelectRoomItem;
        }
    }



    public void OnClickJoinRoom()
    {
        // 방 참여
        PhotonNetwork.JoinRoom(_selectRoomName);

    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("방 입장 성공");
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("방 입장 실패" + message);
    }


    private void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    } // 참여 버튼 클릭시 호출되는 함수


}
