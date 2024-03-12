using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;


public class MultiCinemachineCamera : CinemachineCamera
{
    [Space]
    [Header("Multi Play")]
    [SerializeField] private PhotonView _photonView;



    protected override void Update()
    {
        if(_photonView.IsMine)
            base.Update();
    }
}
