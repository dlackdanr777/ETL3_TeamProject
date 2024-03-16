using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MultiCharacter : Character
{
    [SerializeField] private PhotonView _view;

    protected override void Start()
    {
        base.Start();

        if (!_view.IsMine)
            return;

        _cameraTarget = FindAnyObjectByType<CameraTarget>();
        _cameraTarget.Init(transform);
        cameraTransform = Camera.main.transform;
    }


    protected override void Update()
    {
        animator.SetInteger("State", (int)_state);

        if (!_view.IsMine)
            return;

        base.Update();
    }


    protected override void FixedUpdate()
    {
        if (!_view.IsMine)
            return;

        base.FixedUpdate();
    }



    public override void ChangeState(CharacterState state)
    {
        base.ChangeState(state);
        _view.RPC("MultiChangeState", RpcTarget.All, state);
    }


    public override void ChangeApplyRootMotion(bool value)
    {
        base.ChangeApplyRootMotion(value);
        _view.RPC("MultiChangeApplyRootMotion", RpcTarget.Others, false);
    }


    [PunRPC]
    private void MultiChangeState(CharacterState state)
    {
        _state = state;
    }


    [PunRPC]
    private void MultiChangeApplyRootMotion(bool value)
    {
        animator.applyRootMotion = value;
    }
}
