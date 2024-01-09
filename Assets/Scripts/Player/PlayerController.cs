using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour,IHp
{
    Animator animator;
    [SerializeField] private float _maxHp;
    public float MaxHp => _maxHp;
    [SerializeField] private float _minHp;
    public float MinHp => _minHp;

    private float _hp;
    public float Hp => _hp;

    public event Action<object, float> OnHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action OnHpMax;
    public event Action OnHpMin;

    public void Start()
    {
        animator = GetComponent<Animator>();
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
        _hp = Mathf.Clamp(_hp - value, _minHp, _maxHp);

        OnHpChanged?.Invoke(subject, value);
        OnHpDepleted?.Invoke(subject, value);

        if (10 > value)
        {
            animator.SetTrigger("hit");
        }

        if (_hp == _maxHp)
            OnHpMin?.Invoke();
    }

    
  
}
