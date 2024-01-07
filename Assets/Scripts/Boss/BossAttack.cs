using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    protected float _damage;

    protected BossController _boss;

    public virtual void Init(BossController boss, float damage)
    {
        _boss = boss;
        _damage = damage;
    }

    public abstract void StartAttack();

    public abstract void EndAttack();
}
