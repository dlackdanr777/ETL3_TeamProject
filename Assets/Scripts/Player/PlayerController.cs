using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public enum PlayerSoundType
{
    FootStepWalk,
    Dash,
    Hit,
    Attack1,
    Attack2,
    Attack3,
    Attack4,
}


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

    public AudioSource AudioSource => throw new NotImplementedException();

    public float TmpVolume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    [SerializeField] private float _sta;


    [Space]
    [Header("Components")]
    [SerializeField] private AudioSource _footStepSource;
    [SerializeField] private AudioSource _weaponSource;
    [SerializeField] private AudioSource _bodySource;

    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _walkSound;
    [SerializeField] private AudioClip _dashSound;
    [SerializeField] private AudioClip[] _hitSounds;
    [SerializeField] private AudioClip _attack1Sound;
    [SerializeField] private AudioClip _attack2Sound;
    [SerializeField] private AudioClip _attack3Sound;
    [SerializeField] private AudioClip _attack4Sound;


    [Space]
    public bool isHittable;
    public bool moveable;
    bool isHit;


    public float staminaRecovery;
    public float attackStamina;
    public float rollStamina;
    public float runStamina;
    public float skillStamina;
    public float skillDamage;

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


    public void PlaySound(PlayerSoundType type)
    {
        switch (type)
        {
            case PlayerSoundType.FootStepWalk:
                _footStepSource.clip = _walkSound;
                _footStepSource.Play();
                break;

            case PlayerSoundType.Dash:
                _footStepSource.clip = _dashSound;
                _footStepSource.Play();
                break;

            case PlayerSoundType.Hit:
                int randInt = UnityEngine.Random.Range(0, _hitSounds.Length);
                _bodySource.clip = _hitSounds[randInt];
                _bodySource.Play();
                break;

            case PlayerSoundType.Attack1:
                _weaponSource.clip = _attack1Sound;
                _weaponSource.Play();
                break;

            case PlayerSoundType.Attack2:
                _weaponSource.clip = _attack2Sound;
                _weaponSource.Play();
                break;

            case PlayerSoundType.Attack3:
                _weaponSource.clip = _attack3Sound;
                _weaponSource.Play();
                break;

            case PlayerSoundType.Attack4:
                _weaponSource.clip = _attack4Sound;
                _weaponSource.Play();
                break;



        }
    }
}
