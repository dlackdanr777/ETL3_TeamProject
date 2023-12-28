using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class BossController : MonoBehaviour
{
    [Header("Ability")]
    [SerializeField] private float _moveSpeed;

    [Space]
    [Header("AI")]
    [SerializeField] private float _waitTimeValue;
    [SerializeField] private float _aiUpdateTimeValue;
    

    private BossAI _bossAI;

    private Animator _animator;
    
    [SerializeField] private BossAIState _state;

    private float _waitTimer;


    void Start()
    {
        _bossAI = new BossAI(this);

        _animator = GetComponent<Animator>();

        SetWaitTime();
        InvokeRepeating("AIUpdate", _aiUpdateTimeValue, _aiUpdateTimeValue);
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
        Debug.Log(_waitTimer);

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
