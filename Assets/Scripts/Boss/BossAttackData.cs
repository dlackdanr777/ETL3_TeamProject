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

    [Space]

    [Tooltip("최대 사거리")]
    [SerializeField] private float _maxRange;
    public float MaxRange => _maxRange;

    [Tooltip("최소사거리")]
    [SerializeField] private float _minRange;
    public float MinRange => _minRange;

    [Space]

    [Tooltip("공격 활성화 프레임")]
    [SerializeField] private int _startFrame;
    public int StartFrame => _startFrame;

    [Tooltip("공격 비 활성화 프레임(원거리 경우 상관 없음)")]
    [SerializeField] private int _endFrame;
    public int EndFrame => _endFrame;

    [Space]

    [Tooltip("공격 클래스")]
    [SerializeField] private BossAttack _attackClass;


    [Space]

    [Tooltip("공격 유형")]
    [SerializeField] private BossAttackState _attackState;
    public BossAttackState AttackState => _attackState;


    private BossController _boss;
    public void Init(BossController boss)
    {
        _boss = boss;
    }

    public void Start()
    {
        _attackClass.Init(_boss.Power * _timesDamage);
        _attackClass.StartAttack();
    }

    public void End()
    {
        _attackClass.EndAttack();
    }

}
