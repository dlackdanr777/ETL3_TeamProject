using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    protected float _damage;

    public virtual void Init(float damage)
    {
        _damage = damage;
    }

    public abstract void StartAttack();

    public abstract void EndAttack();
}
