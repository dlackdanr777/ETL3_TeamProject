
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class MultiBossController : BossController
{
    [Space]
    [Tooltip("Photon")]
    [SerializeField] private PhotonView _view;


    protected override void Start()
    {
        base.Start();

        if(!PhotonNetwork.IsMasterClient)
            CancelInvoke("AIUpdate");
    }


    public override void ChangeAiState(BossAIState nextState)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _view.RPC("ChangeStateMulti", RpcTarget.All, nextState);
        }

    }


    [PunRPC]
    private void ChangeStateMulti(BossAIState nextState)
    {
        _state = nextState;
    }
}
