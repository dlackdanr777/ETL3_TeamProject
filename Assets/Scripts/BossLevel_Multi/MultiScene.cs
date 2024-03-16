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

    private PhotonView _view;
    private int _index;


    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }


    private void Start()
    {
        GameObject obj = PhotonNetwork.Instantiate("Player(Multi)", _spawnPos[_index].position, Quaternion.identity);
        Character player = obj.GetComponent<Character>();
        UIPlayer uiPlayer = Instantiate(_uiPlayerPrefab);
        uiPlayer.Init(player);

        _view.RPC("AddIndex", RpcTarget.All);
    }


    [PunRPC]
    private void AddIndex()
    {
        _index++;
        Debug.Log(_index);
    }
}
