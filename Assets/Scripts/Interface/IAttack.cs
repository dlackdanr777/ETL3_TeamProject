using System;

public interface IAttack
{
    float DamageValue { get; }

    event Action OnTargetDamaged;

    void AttackTarget(IHp iHp, float aomunt);
}
