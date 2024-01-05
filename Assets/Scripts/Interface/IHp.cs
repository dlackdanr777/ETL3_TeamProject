using System;

public interface IHp
{
    public float Hp { get; }
    public float MaxHp { get; }
    public float MinHp { get; }

    public event Action<object, float> OnHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action OnHpMax;

    public event Action OnHpMin;

    public void RecoverHp(object subject, float value);
    public void DepleteHp(object subject, float value);
}
