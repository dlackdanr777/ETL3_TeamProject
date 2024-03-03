using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private Button _button;

    [SerializeField] private GameObject _selectImage;

    [SerializeField] private TextMeshProUGUI _roomNameText;

    [SerializeField] private TextMeshProUGUI _maxNumText;

    //Ŭ���Ǿ����� ȣ��Ǵ� �Լ�
    public Action<string> OnClickHandler;


    private void Start()
    {
        _selectImage.gameObject.SetActive(false);
    }



    public void SetInfo(string roomName, int currPlayer, int maxPlayer)
    {
        name = roomName;
        _roomNameText.text = roomName;
        _maxNumText.text = "(" + currPlayer + " /" + maxPlayer + ")";

        _selectImage.SetActive(false);
        _button.onClick.AddListener(OnClick);
    }


    public void OnClick()
    {
        //���� onDelegate �� ���� ����ִٸ� ����
        OnClickHandler?.Invoke(name);
        _selectImage.SetActive(true);
        ////InputRoomName ã�ƿ���
        //GameObject go =GameObject.Find("InputRoomName");
        ////ã�ƿ� ���ӿ�����Ʈ���� InputField ������Ʈ ��������
        //InputField inputField = go.GetComponent<InputField>();
        ////������ ������Ʈ���� text ���� ���� �̸����� �����ϱ�
        //inputField.text = name;
    }

    public void Unselected()
    {
        _selectImage.SetActive(false);
    }
}