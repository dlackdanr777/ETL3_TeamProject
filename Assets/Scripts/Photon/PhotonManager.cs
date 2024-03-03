using Photon.Pun;
using UnityEngine;


namespace Muks.Photon
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        private string _gameVersion = "1";


        private static PhotonManager instance;

        public static PhotonManager Instance
        {
            get
            {
                if (!instance)
                {
                    GameObject obj;
                    obj = GameObject.Find(typeof(PhotonManager).Name);
                    if (!obj)
                    {
                        obj = new GameObject(typeof(PhotonManager).Name);
                        instance = obj.AddComponent<PhotonManager>();
                    }
                    else
                    {
                        instance = obj.GetComponent<PhotonManager>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            GameObject obj = GameObject.Find(typeof(PhotonManager).Name);

            if (!obj || !instance)
            {
                instance = gameObject.GetComponent<PhotonManager>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        /// <summary>���� ������ ��û�ϴ� �Լ�</summary>
        public void MasterServerConnect(string nickName)
        {
            // ������ ���� ���� ��û
            PhotonNetwork.NickName = nickName;
            PhotonNetwork.GameVersion = _gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }


        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("���� ���� ����");

            //�κ�����
            PhotonNetwork.JoinLobby();
        }


        //Lobby ������ ���������� ȣ��Ǵ� �Լ�
        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();

            //�κ� ������ �̵�
            PhotonNetwork.LoadLevel("MultiplayLobby");

            print("�κ� ���� ����");

            Debug.Log(PhotonNetwork.NickName);
        }
    }

}
