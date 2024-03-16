using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDamageDealer : DamageDealer
{
    private PhotonView _view;

    public override void Init(PhotonView view)
    {
        _view = view;
    }

    protected override void Update()
    {
        if (!_view.IsMine)
            return;

        base.Update();
    }

}
