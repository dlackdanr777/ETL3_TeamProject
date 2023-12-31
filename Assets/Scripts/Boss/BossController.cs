using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Playables;
using UnityEngine;


[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class BossController : MonoBehaviour
{
    [Header("Ability")]

    [SerializeField] private float _power;
    public float Power => _power;

    [SerializeField] private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;

    [SerializeField] private float _rotateSpeed;
    public float RotateSpeed => _rotateSpeed;

    [Space]
    [Header("Attack")]
    [SerializeField] private BossAttackData[] _attackDatas;

    [Space]
    [Header("AI")]
    [SerializeField] private float _waitTimeValue;
    [SerializeField] private float _aiUpdateTimeValue;
    public float AIUpdateTime => _aiUpdateTimeValue;
    [SerializeField] private float _attackWaitTimeValue;
    public float AttackWaitTime => _attackWaitTimeValue;
    

    private BossAI _bossAI;
    private BossStateMachineBehaviour[] _stateMachines;
    private BossAttackStateBehaviour[] _attackMachines;

    private Animator _animator;
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    
    [SerializeField] private BossAIState _state;

    [SerializeField] private GameObject _target;
    public GameObject Target => _target;

    public float TargetDistance =>
        Vector3.Distance(new Vector3(_target.transform.position.x, 0, _target.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));

    private float _waitTimer;


    void Start()
    {
        _bossAI = new BossAI(this);

        Init();
        SetWaitTime();
        InvokeRepeating("AIUpdate", _aiUpdateTimeValue, _aiUpdateTimeValue);
    }

    private void Init()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _stateMachines = _animator.GetBehaviours<BossStateMachineBehaviour>();
        _attackMachines = _animator.GetBehaviours<BossAttackStateBehaviour>();

        foreach (BossStateMachineBehaviour behaviour in _stateMachines)
        {
            behaviour.Init(this);
        }

        foreach(BossAttackData data in _attackDatas)
        {
            data.Init(this);
            foreach (BossAttackStateBehaviour behaviour in _attackMachines)
            {
                if(data.AttackState == behaviour.AttackState)
                {
                    behaviour.Init(this, data);
                    break;
                }
            }
        }

    }

    private void Update()
    {
        UpdateTimer();
        _animator.SetInteger("State", (int)_state);
    }

    private void AIUpdate()
    {
        _bossAI.AIUpdate();
    }


    private void UpdateTimer()
    {
        if (_waitTimer <= 0)
            return;

        _waitTimer -= Time.deltaTime;
    }


    public bool CheckWaitTime()
    {
        return _waitTimer <= 0;
    }


    public void SetWaitTime()
    {
        _waitTimer = _waitTimeValue;
    }


    public BossAttackData GetPossibleAttackData()
    {
        List<BossAttackData> possibleSkillDataList = new List<BossAttackData>();
        foreach (BossAttackData data in _attackDatas)
        {
            bool isWithInRange = TargetDistance <= data.MaxRange && data.MinRange <= TargetDistance;
            if (isWithInRange)
                possibleSkillDataList.Add(data);
        }

        if (possibleSkillDataList.Count == 0)
            return null;

        int randInt = Random.Range(0, possibleSkillDataList.Count);
        return possibleSkillDataList[randInt];
    }


    public void ChangeAiState(BossAIState nextState)
    {
        _state = nextState;
    }

    public void SetAnimatorAttackValue(BossAttackState nextState)
    {
        _animator.SetInteger("AttackState", (int)nextState);
    }
}
