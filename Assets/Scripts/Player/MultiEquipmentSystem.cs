using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEquipmentSystem : EquipmentSystem
{
    [SerializeField] private PhotonView _view;

    protected override void Start()
    {
        base.Start();
        currentWeaponInhand.GetComponentInChildren<DamageDealer>().Init(_view);
    }
}
