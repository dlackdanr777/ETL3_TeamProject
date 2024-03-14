using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class MultiScene : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPos;

    private PhotonView _view;
    private int _index;


    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }


    private void Start()
    {
        PhotonNetwork.Instantiate("Player(Multi)", _spawnPos[_index].position, Quaternion.identity);

        _view.RPC("AddIndex", RpcTarget.All);
    }


    [PunRPC]
    private void AddIndex()
    {
        _index++;
        Debug.Log(_index);
    }
}
