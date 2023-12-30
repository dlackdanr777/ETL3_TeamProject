using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;


[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class BossController : MonoBehaviour
{
    [Header("Ability")]
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;

    [SerializeField] private float _rotateSpeed;
    public float RotateSpeed => _rotateSpeed;

    [Space]
    [Header("AI")]
    [SerializeField] private float _waitTimeValue;
    [SerializeField] private float _aiUpdateTimeValue;
    public float AIUpdateTime => _aiUpdateTimeValue;
    [SerializeField] private float _attackWaitTimeValue;
    public float AttackWaitTime => _attackWaitTimeValue;
    

    private BossAI _bossAI;
    private BossStateMachineBehaviour[] _stateMachines;

    private Animator _animator;
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    
    [SerializeField] private BossAIState _state;

    [SerializeField] private GameObject _target;
    public GameObject Target => _target;

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
        foreach(BossStateMachineBehaviour behaviour in _stateMachines)
        {
            behaviour.Init(this);
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


    public void ChangeAiState(BossAIState nextState)
    {
        _state = nextState;
    }
}
