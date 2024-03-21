using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class MultiScene : MonoBehaviour
{
    [Space]
    [Header("Player UI")]
    [SerializeField] private UIPlayer _uiPlayerPrefab;


    [Space]
    [Header("Player Spawn Position")]
    [SerializeField] private Transform[] _spawnPos;

    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _backgroundAudio;

    private PhotonView _view;
    private int _index;


    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }


    private void Start()
    {
        LoadingSceneManager.OnLoadSceneHandler += OnSceneChanged;
        GameManager.Instance.LockCursor();

        GameObject obj = PhotonNetwork.Instantiate("Player(Multi)", _spawnPos[_index].position, Quaternion.identity);
        Character player = obj.GetComponent<Character>();
        UIPlayer uiPlayer = Instantiate(_uiPlayerPrefab);
        uiPlayer.Init(player);

        _view.RPC("AddIndex", RpcTarget.All);

        SoundManager.Instance.PlayBackgroundMusic(_backgroundAudio);
    }


    [PunRPC]
    private void AddIndex()
    {
        _index++;
        Debug.Log(_index);
    }


    private void OnSceneChanged()
    {
        if(PhotonNetwork.InLobby)
            PhotonNetwork.LeaveLobby();


        LoadingSceneManager.OnLoadSceneHandler -= OnSceneChanged;
    }
}
