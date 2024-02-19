using System;

public interface IStamina
{
    public float Sta { get; }
    public float MaxSta { get; }
    public float MinSta { get; }

    public event Action<object, float> OnStaChanged;
    public event Action<object, float> OnStaRecoverd;
    public event Action<object, float> OnStaDepleted;
    public event Action OnStaMax;

    public event Action OnStaMin;

    public void RecoverSta(object subject, float value);
    public void DepleteSta(object subject, float value);
}
