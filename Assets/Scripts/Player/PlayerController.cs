using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour,IHp,IStamina
{
    Animator animator;
    [SerializeField] private float _maxHp;
    public float MaxHp => _maxHp;
    [SerializeField] private float _minHp;
    public float MinHp => _minHp;

    [SerializeField] private float _hp;
    public float Hp => _hp;


    public float MaxSta => _maxSta;
    [SerializeField] private float _maxSta;

    public float MinSta => _minSta;

    [SerializeField] private float _minSta;
    public float Sta => _sta;

    [SerializeField] private float _sta;

    public bool isHittable;


    public float staminaRecovery;
    public float attackStamina;
    public float rollStamina;
    public float runStamina;
    public float skillStamina;

    public event Action<object, float> OnHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action OnHpMax;
    public event Action OnHpMin;
    public event Action<object, float> OnStaChanged;
    public event Action<object, float> OnStaRecoverd;
    public event Action<object, float> OnStaDepleted;
    public event Action OnStaMax;
    public event Action OnStaMin;


    private void Start()
    {
        animator = GetComponent<Animator>();
        _sta = 0;
        isHittable = true;

    }

    private void FixedUpdate()
    {
        RecoverSta(_sta, staminaRecovery*Time.deltaTime);
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
        if (isHittable)
        {
            _hp = Mathf.Clamp(_hp - value, _minHp, _maxHp);

            OnHpChanged?.Invoke(subject, value);
            OnHpDepleted?.Invoke(subject, value);

            if (4 < value)
            {
                animator.SetTrigger("hit");
            }

            if (_hp == _maxHp)
                OnHpMin?.Invoke();
        }
      
    }

    public void RecoverSta(object subject, float value)
    {
        _sta = Mathf.Clamp(_sta + value, _minSta, _maxSta);

        OnStaChanged?.Invoke(subject, value);
        OnStaRecoverd?.Invoke(subject, value);

        if(_sta==_maxSta)
            OnStaMax?.Invoke();
    }

    public void DepleteSta(object subject, float value)
    {
        _sta = Mathf.Clamp(_sta -value, _minSta, _maxSta);
        OnStaChanged?.Invoke(subject, value);
        OnStaDepleted?.Invoke(subject, value);

        if (_sta == _maxSta)
            OnStaMin?.Invoke();
    }
}
