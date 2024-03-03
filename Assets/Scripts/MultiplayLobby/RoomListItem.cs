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

    //클릭되었을때 호출되는 함수
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
        //만약 onDelegate 에 무언가 들어있다면 실행
        OnClickHandler?.Invoke(name);
        _selectImage.SetActive(true);
        ////InputRoomName 찾아오기
        //GameObject go =GameObject.Find("InputRoomName");
        ////찾아온 게임오브젝트에서 InputField 컴포넌트 가져오기
        //InputField inputField = go.GetComponent<InputField>();
        ////가져온 컴포넌트에서 text 값을 나의 이름으로 셋팅하기
        //inputField.text = name;
    }

    public void Unselected()
    {
        _selectImage.SetActive(false);
    }
}