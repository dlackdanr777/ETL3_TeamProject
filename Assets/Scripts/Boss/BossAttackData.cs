using JetBrains.Annotations;
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
    Attack6,
    Attack7,
    Attack8,
    Attack9
}

[Serializable]
public class BossAttackFrameData
{
    [Tooltip("공격 활성화 프레임")]
    [SerializeField] private int _startFrame;
    public int StartFrame => _startFrame;

    [Tooltip("공격 비 활성화 프레임(원거리 경우 상관 없음)")]
    [SerializeField] private int _finishedFrame;
    public int FinishedFrame => _finishedFrame;


    private bool _isStarted;
    public bool GetIsStarted => _isStarted;
    public bool SetIsStarted { set { _isStarted = value; } }

    private bool _isFinished;
    public bool GetIsFinished => _isFinished;
    public bool SetIsFinished { set { _isFinished = value; } }
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
    [Tooltip("타격 활성화, 비활성화 프레임 클래스(크기는 타격 횟수)")]
    [SerializeField] private BossAttackFrameData[] _frames;
    public BossAttackFrameData[] Frames => _frames;

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
        _attackClass.Init(_boss, _boss.Power * _timesDamage);
        _attackClass.StartAttack();
    }

    public void End()
    {
        _attackClass.EndAttack();
    }

}
