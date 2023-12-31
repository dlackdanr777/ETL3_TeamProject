using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour, IHp
{
    public float hp => throw new NotImplementedException();

    public float maxHp => throw new NotImplementedException();

    public float minHp => throw new NotImplementedException();

    public event Action<float> onHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action onHpMax;
    public event Action onHpMin;

    public void DepleteHp(object subject, float value)
    {
        Debug.Log("¸ÂÀ½");
    }

    public void RecoverHp(object subject, float value)
    {
        throw new NotImplementedException();
    }


}
