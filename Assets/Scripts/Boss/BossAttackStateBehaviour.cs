using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BossAttackStateBehaviour : StateMachineBehaviour
{

    [SerializeField] private BossAttackState _state;
    public BossAttackState AttackState => _state;

    protected BossController _boss;

    protected BossAttackData _data;

    public void Init(BossController boss, BossAttackData data)
    {
        _boss = boss;
        _data = data;
        Debug.Log("¿¬°áµÊ");
    }


}
