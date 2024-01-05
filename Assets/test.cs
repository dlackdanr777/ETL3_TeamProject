using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour, IHp
{
    public float Hp => throw new NotImplementedException();

    public float MaxHp => throw new NotImplementedException();

    public float MinHp => throw new NotImplementedException();

    public event Action<object, float> OnHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action OnHpMax;
    public event Action OnHpMin;


    public void DepleteHp(object subject, float value)
    {
        Debug.Log("¸ÂÀ½");
    }

    public void RecoverHp(object subject, float value)
    {
        throw new NotImplementedException();
    }


}
