using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IAttack
{
    private float _damageValue;
    public float DamageValue => _damageValue;

    private object _subject;

    private List<GameObject> _hitObjects = new List<GameObject>();

    public event Action OnTargetDamaged;

    

    public void Init(object subject, float damageValue)
    {
        Debug.LogFormat("{0}의 공격력은 {1} 입니다.", subject, damageValue);

        _subject = subject;
        _damageValue = damageValue;
    }

    public void AttackTarget(IHp iHp, float aomunt)
    {
        iHp.DepleteHp(_subject, aomunt);

        OnTargetDamaged?.Invoke();
    }


    private void OnTriggerStay(Collider other)
    {
        GameObject obj = _hitObjects.Find(x => x == other.gameObject);

        if (obj == null)
        {
            if (other.TryGetComponent(out IHp iHp))
            {
                AttackTarget(iHp, _damageValue);
                _hitObjects.Add(other.gameObject);
            }
        }
    }

}
