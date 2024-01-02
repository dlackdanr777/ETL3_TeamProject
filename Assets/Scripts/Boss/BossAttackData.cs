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

    [Space]

    [Tooltip("�ִ� ��Ÿ�")]
    [SerializeField] private float _maxRange;
    public float MaxRange => _maxRange;

    [Tooltip("�ּһ�Ÿ�")]
    [SerializeField] private float _minRange;
    public float MinRange => _minRange;

    [Space]

    [Tooltip("���� Ȱ��ȭ ������")]
    [SerializeField] private int _startFrame;
    public int StartFrame => _startFrame;

    [Tooltip("���� �� Ȱ��ȭ ������(���Ÿ� ��� ��� ����)")]
    [SerializeField] private int _endFrame;
    public int EndFrame => _endFrame;

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
        _attackClass.Init(_boss.Power * _timesDamage);
        _attackClass.StartAttack();
    }

    public void End()
    {
        _attackClass.EndAttack();
    }

}
