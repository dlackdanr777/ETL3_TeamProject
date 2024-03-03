using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private Text _roomInfo;

    //Ŭ���Ǿ����� ȣ��Ǵ� �Լ�
    public Action<string> OnClickHandler;


    public void SetInfo(string roomName, int currPlayer, int maxPlayer)
    {
        name = roomName;
        _roomInfo.text = roomName + '(' + currPlayer + '/' + maxPlayer + ')';
    }


    public void OnClick()
    {
        //���� onDelegate �� ���� ����ִٸ� ����
        OnClickHandler?.Invoke(name);
        ////InputRoomName ã�ƿ���
        //GameObject go =GameObject.Find("InputRoomName");
        ////ã�ƿ� ���ӿ�����Ʈ���� InputField ������Ʈ ��������
        //InputField inputField = go.GetComponent<InputField>();
        ////������ ������Ʈ���� text ���� ���� �̸����� �����ϱ�
        //inputField.text = name;
    }
}