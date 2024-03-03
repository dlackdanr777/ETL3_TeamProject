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


        /// <summary>서버 접속을 요청하는 함수</summary>
        public void MasterServerConnect(string nickName)
        {
            // 마스터 서버 접속 요청
            PhotonNetwork.NickName = nickName;
            PhotonNetwork.GameVersion = _gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }


        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("서버 접속 성공");

            //로비진입
            PhotonNetwork.JoinLobby();
        }


        //Lobby 진입을 성공했으면 호출되는 함수
        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();

            //로비 씬으로 이동
            PhotonNetwork.LoadLevel("MultiplayLobby");

            print("로비 진입 성공");

            Debug.Log(PhotonNetwork.NickName);
        }
    }

}
