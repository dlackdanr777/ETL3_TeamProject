using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private UIBar _healthBar;
    [SerializeField] private UIBar _steminaBar;

    private Character _player;


    public void Init(Character player)
    {
        _player = player;

        _healthBar.SetBarAfterNoAnime(_player.playerController.MaxHp, _player.playerController.Hp);
        _steminaBar.SetBarAfterNoAnime(_player.playerController.MaxSta, _player.playerController.Sta);
        _player.playerController.OnHpChanged += OnHpChanged;
        _player.playerController.OnStaChanged += OnSteminaChanged;

    }


    private void OnHpChanged(object subject, float value)
    {
        _healthBar.SetBar(_player.playerController.MaxHp, _player.playerController.Hp);
    }


    private void OnSteminaChanged(object subject, float value)
    {
        _steminaBar.SetBarAfterNoAnime(_player.playerController.MaxSta, _player.playerController.Sta);
    }


    private void OnDestroy()
    {
        _player.playerController.OnHpChanged -= OnHpChanged;
        _player.playerController.OnStaChanged -= OnSteminaChanged;
    }
}
