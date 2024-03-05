using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Runtime.InteropServices.WindowsRuntime;

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

    [Tooltip("���� Ž�� ����")]
    [SerializeField] private float _explorationScope;

    [Tooltip("���� Ž�� ���� �ð�")]
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

        foreach (BossAttackData data in _attackDatas)
        {
            data.Init(this);
            foreach (BossAttackStateBehaviour behaviour in _attackMachines)
            {
                if (data.AttackState == behaviour.AttackState)
                {
                    Debug.Log(behaviour.name + ", " + data.AttackState);
                    behaviour.Init(this, data);
                }
            }
        }

        _hp = _maxHp;
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
            DepleteHp("â��", 50);
            Debug.LogFormat("���� ü���� {0} �Դϴ�.", _hp);
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


    public bool CheckWaitTime()
    {
        return _waitTimer <= 0;
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


    public BossAIState GetAIState()
    {
        return _state;
    }


    public void SetAnimatorAttackValue(BossAttackState nextState)
    {
        _animator.SetInteger("AttackState", (int)nextState);
    }


    /// <summary>���� �ֱ�� Ÿ���� Ž���ϴ� �Լ�</summary> 
    public void UpdateFindTarget()
    {
        _currentExplorationTimer -= Time.deltaTime;

        if(_currentExplorationTimer <= 0)
        {
            FindTarget(0.02f);
            _currentExplorationTimer = _explorationTime;
        }
    }

    /// <summary>�÷��̾���Ʈ�ѷ� ������Ʈ�� ������ Ÿ���� Ȯ���� Target������ �����ϴ� �Լ�</summary>
    public void FindTarget(float time)
    {
        if (_findTargetRoutine != null)
            StopCoroutine(_findTargetRoutine);

        _findTargetRoutine = StartCoroutine(FindTargetRoutine(time));
    }


    private IEnumerator FindTargetRoutine(float time)
    {
        yield return YieldCache.WaitForSeconds(time);

        Debug.Log("Ž����");
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

        //������϶��� �����Ѵ�.
        if (_state == BossAIState.Guard)
        {
            _animator.Play("DefenseHit", -1, 0);
            return;
        }

        _hp = Mathf.Clamp(_hp - value, _minHp, _maxHp);

        OnHpChanged?.Invoke(subject, value);
        OnHpDepleted?.Invoke(subject, value);

        if (_hp == _minHp)
        {
            _state = BossAIState.Die;
            OnHpMin?.Invoke();
        }

    }
}
