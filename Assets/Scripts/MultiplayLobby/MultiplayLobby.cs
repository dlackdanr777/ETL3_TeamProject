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

    // �� ����� ������ �ִ� Dictionary ����
    private Dictionary<string, RoomInfo> _dicRoomInfo = new Dictionary<string, RoomInfo>();

    private string _selectRoomName;


    void Start()
    {
        _joinRoomButton.onClick.AddListener(OnClickJoinRoom);
    }


    //�� ����� ��ȭ�� ���� �� ȣ��Ǵ� �Լ�
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
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
            GameObject go = Instantiate(_roomListItem, rtContent);
            //������ item���� RoomListItem ������Ʈ�� �����´�.
            RoomListItem item = go.GetComponent<RoomListItem>();
            //������ ������Ʈ�� ������ �ִ� SetInfo �Լ� ����
            item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //item Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ� ���
            item.OnClickHandler = SelectRoomItem;
        }
    }



    public void OnClickJoinRoom()
    {
        // �� ����
        PhotonNetwork.JoinRoom(_selectRoomName);

    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("�� ���� ����");
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("�� ���� ����" + message);
    }


    private void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    } // ���� ��ư Ŭ���� ȣ��Ǵ� �Լ�


}
