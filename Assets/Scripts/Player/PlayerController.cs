using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _maxHp;

    

    public float hp => _hp;
    private float _hp;
    public float maxHp => _maxHp;

    private float _minHp;
    public float minHp => _minHp;

    public event Action<object, float> onHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action OnHpMax;
    public event Action OnHpMin;

    public void RecoverHp(object subject, float value)
    {
        _hp = Mathf.Clamp(_hp + value, _minHp, _maxHp);

        onHpChanged?.Invoke(subject, value);
        OnHpRecoverd?.Invoke(subject, value);

        if (_hp == _maxHp)
            OnHpMax?.Invoke();
    }

    public void DepleteHp(object subject, float value)
    {
        _hp = Mathf.Clamp(_hp - value, _minHp, _maxHp);

        onHpChanged?.Invoke(subject, value);
        OnHpDepleted?.Invoke(subject, value);

        if (_hp == _maxHp)
            OnHpMin?.Invoke();
    }

    
  
}
