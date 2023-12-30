using System;
using UnityEngine;


/// <summary> 보스 공격 유형 </summary>
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
    [Tooltip("이름")]
    [SerializeField] private string _name;
    public string Name => _name;

    [Tooltip("공격력 배수")]
    [SerializeField] private float _timesDamage;
    public float TimesDamage => _timesDamage;

    [Tooltip("공격 유형")]
    [SerializeField] private BossAttackState _attackState;
    public BossAttackState AttackState => _attackState;
}
