using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class BossController : MonoBehaviour, IHp
{
    [Header("Ability")]

    [SerializeField] private float _power;
    public float Power => _power;

    [SerializeField] private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;

    [SerializeField] private float _rotateSpeed;
    public float RotateSpeed => _rotateSpeed;

    [SerializeField] private float _maxHp;
    public float MaxHp => _maxHp;

    [SerializeField] private float _minHp;
    public float MinHp => _minHp;

    private float _hp;
    public float Hp => _hp;

    [Tooltip("보스 탐색 범위")]
    [SerializeField] private float _explorationScope;

    [Tooltip("보스 탐색 갱신 시간")]
    [SerializeField] private float _explorationTime;

    [Space]
    [Header("Attack")]
    [SerializeField] private BossAttackData[] _attackDatas;

    [Space]
    [Header("AI")]
    [SerializeField] private float _waitTimeValue;

    [SerializeField] private float _aiUpdateTimeValue;
    public float AIUpdateTime => _aiUpdateTimeValue;

    [SerializeField] private float _attackWaitTimeValue;

    private float _attackWaitTimer;


    [Space]
    [Header("Effects")]
    [SerializeField] private ParticleSystem _hitParticle;


    private BossAI _bossAI;

    private BossStateMachineBehaviour[] _stateMachines;

    private BossAttackStateBehaviour[] _attackMachines;

    private Animator _animator;

    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;

    protected BossAIState _state;

    private GameObject _target;
    public GameObject Target => _target;

    private float _currentExplorationTimer;

    public float TargetDistance =>
        Vector3.Distance(new Vector3(_target.transform.position.x, 0, _target.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));

    public event Action<object, float> OnHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action OnHpMax;
    public event Action OnHpMin;

    private float _waitTimer;

    private Coroutine _findTargetRoutine;


    protected virtual void Start()
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

        foreach (BossAttackData data in _attackDatas)
        {
            data.Init(this);
            foreach (BossAttackStateBehaviour behaviour in _attackMachines)
            {
                if (data.AttackState == behaviour.AttackState)
                {
                    behaviour.Init(this, data);
                }
            }
        }

        _hp = _maxHp;
        _attackWaitTimer = _attackWaitTimeValue;
    }


    private void Update()
    {
        _animator.SetInteger("State", (int)_state);

        if (_state == BossAIState.Die)
            return;

        UpdateTimer();
        UpdateFindTarget();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            DepleteHp("창묵", 50);
            Debug.LogFormat("현재 체력은 {0} 입니다.", _hp);
        }

    }


    private void AIUpdate()
    {
        if (_state == BossAIState.Die)
        {
            CancelInvoke("AIUpdate");
            return;
        }

        _bossAI.AIUpdate();
    }


    private void UpdateTimer()
    {
        if (_waitTimer <= 0)
            return;

        _waitTimer -= Time.deltaTime;
    }


    public bool CheckStateChangeEnabled()
    {
        bool isWaitTimeEnd = _waitTimer <= 0;
        bool changeStateEnabled = _state == BossAIState.Idle || _state == BossAIState.Tracking 
            || _state == BossAIState.Reconnaissance || _state == BossAIState.Guard;

        return isWaitTimeEnd && changeStateEnabled;
    }


    /// <summary>공격 대기 시간이 전부 지났나 확인하는 함수</summary>
    public bool CheckAttackWaitTime()
    {
        if (_attackWaitTimer <= 0)
            return true;

        return false;
    }


    /// <summary>공격 대기 시간을 설정 하는 함수</summary>
    public void SetAttackTime()
    {
        _attackWaitTimer = _attackWaitTimeValue;
    }



    /// <summary>공격 대기 타이머를 감소 시키는 함수</summary>
    public void DepleteAttackTime(float value)
    {
        _attackWaitTimer -= _attackWaitTimer <= 0 ? 0 : value;
    }


    public void SetWaitTime()
    {
        _waitTimer = _waitTimeValue;
    }


    public void SetWaitTime(float value)
    {
        _waitTimer = value;
    }


    public BossAttackData GetPossibleAttackData()
    {
        List<BossAttackData> possibleSkillDataList = new List<BossAttackData>();
        foreach (BossAttackData data in _attackDatas)
        {
            
            bool isWithInRange = TargetDistance <= data.MaxRange && data.MinRange <= TargetDistance && data.StateChangeEnabled;
            if (isWithInRange)
                possibleSkillDataList.Add(data);

        }

        if (possibleSkillDataList.Count == 0)
            return null;

        int randInt = Random.Range(0, possibleSkillDataList.Count);
        return possibleSkillDataList[randInt];
    }


    public virtual void ChangeAiState(BossAIState nextState)
    {
        _state = nextState;
    }


    public BossAIState GetAIState()
    {
        return _state;
    }


    public void SetAnimatorAttackValue(BossAttackState nextState)
    {
        _animator.SetInteger("AttackState", (int)nextState);
    }


    /// <summary>일정 주기로 타겟을 탐색하는 함수</summary> 
    public void UpdateFindTarget()
    {
        _currentExplorationTimer -= Time.deltaTime;

        if(_currentExplorationTimer <= 0)
        {
            FindTarget(0.02f);
            _currentExplorationTimer = _explorationTime;
        }
    }


    /// <summary>플레이어컨트롤러 컴포넌트를 보유한 타겟을 확인해 Target변수를 변경하는 함수</summary>
    public void FindTarget(float time)
    {
        if (_findTargetRoutine != null)
            StopCoroutine(_findTargetRoutine);

        _findTargetRoutine = StartCoroutine(FindTargetRoutine(time));
    }


    private IEnumerator FindTargetRoutine(float time)
    {
        yield return YieldCache.WaitForSeconds(time);

        _currentExplorationTimer = _explorationTime;

        Collider[] hitCollider = Physics.OverlapSphere(transform.position, _explorationScope);
        PlayerController player = null;
        List<PlayerController> playerList = new List<PlayerController>();

        for (int i = 0, count = hitCollider.Length; i < count; i++)
        {
            if (hitCollider[i].TryGetComponent(out player))
            {
                playerList.Add(player);
            }
        }

        if (playerList.Count == 0)
            yield break;

        int targetPlayerIndex = Random.Range(0, playerList.Count);
        _target = playerList[targetPlayerIndex].gameObject;
    }


    public void RecoverHp(object subject, float value)
    {
        _hp = Mathf.Clamp(_hp + value, _minHp, _maxHp);

        OnHpChanged?.Invoke(subject, value);
        OnHpRecoverd?.Invoke(subject, value);

        if (_hp == _maxHp)
            OnHpMax?.Invoke();
    }


    public void DepleteHp(object subject, float value)
    {
        if (_hp == _minHp)
            return;

        _hitParticle.Play();

        //방어중일때는 리턴한다.
        if (_state == BossAIState.Guard)
        {
            _animator.Play("DefenseHit", -1, 0);
            return;
        }

        _hp = Mathf.Clamp(_hp - value, _minHp, _maxHp);

        OnHpChanged?.Invoke(subject, value);
        OnHpDepleted?.Invoke(subject, value);
        Debug.Log("맞음 _hp");
        if (_hp == _minHp)
        {
            _state = BossAIState.Die;
            OnHpMin?.Invoke();
        }

    }
}
