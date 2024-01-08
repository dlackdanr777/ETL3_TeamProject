using JetBrains.Annotations;
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
    Attack6,
    Attack7,
    Attack8,
    Attack9
}

[Serializable]
public class BossAttackFrameData
{
    [Tooltip("���� Ȱ��ȭ ������")]
    [SerializeField] private int _startFrame;
    public int StartFrame => _startFrame;

    [Tooltip("���� �� Ȱ��ȭ ������(���Ÿ� ��� ��� ����)")]
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
    [Tooltip("�̸�")]
    [SerializeField] private string _name;
    public string Name => _name;

    [Tooltip("���ݷ� ���")]
    [SerializeField] private float _timesDamage;
    public float TimesDamage => _timesDamage;

    [Space]

    [Tooltip("�ִ� ��Ÿ�")]
    [SerializeField] private float _maxRange;
    public float MaxRange => _maxRange;

    [Tooltip("�ּһ�Ÿ�")]
    [SerializeField] private float _minRange;
    public float MinRange => _minRange;

    [Space]
    [Tooltip("Ÿ�� Ȱ��ȭ, ��Ȱ��ȭ ������ Ŭ����(ũ��� Ÿ�� Ƚ��)")]
    [SerializeField] private BossAttackFrameData[] _frames;
    public BossAttackFrameData[] Frames => _frames;

    [Space]

    [Tooltip("���� Ŭ����")]
    [SerializeField] private BossAttack _attackClass;


    [Space]

    [Tooltip("���� ����")]
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
