using System;
using UnityEngine;


/// <summary> ���� ���� ���� </summary>
public enum BossAttackState
{
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    Attack5,
}


[Serializable]
public class BossAttackData
{
    [Tooltip("�̸�")]
    [SerializeField] private string _name;
    public string Name => _name;

    [Tooltip("���ݷ� ���")]
    [SerializeField] private float _timesDamage;
    public float TimesDamage => _timesDamage;

    [Tooltip("���� ����")]
    [SerializeField] private BossAttackState _attackState;
    public BossAttackState AttackState => _attackState;
}
