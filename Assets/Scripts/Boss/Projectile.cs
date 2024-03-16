using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Projectile : MonoBehaviour, IAttack
{
    //데미지 갱신 간격(ex. 0.1일 경우 닿고 있는 모든 대상 0.1초마다 데미지)
    private float _damageInterval = 1;

    //타격 데미지
    private float _damageValue;
    public float DamageValue => _damageValue;

    private object _subject;

    private Dictionary<GameObject, float> _hitObjectDic = new Dictionary<GameObject, float>();

    //Dic에서 삭제될 Obj를 보관해두는 리스트
    private List<GameObject> _removeObjectList = new List<GameObject>();

    public event Action OnTargetDamaged;

    public void Init(object subject, float damageValue, float damageInterval)
    {
        _subject = subject;
        _damageValue = damageValue;
        _damageInterval = damageInterval;
    }


    public void AttackTarget(IHp iHp, float aomunt)
    {

        iHp.DepleteHp(_subject, aomunt);
        OnTargetDamaged?.Invoke();
    }

    private void Update()
    {
        _removeObjectList.Clear();

        //TODO: 수정 예정 (ToList를 사용해 메모리를 과다 사용할 수 있음)
        foreach (GameObject obj in _hitObjectDic.Keys.ToList())
        {
            // 아닐 경우 Time.deltaTime만큼만 뺀다.
            _hitObjectDic[obj] = _hitObjectDic[obj] - Time.deltaTime;

            // 만약 _damageInterval 시간을 지났다면 리스트에 추가
            if (_hitObjectDic[obj] <= 0)
                _removeObjectList.Add(obj);
        }

        foreach (GameObject obj in _removeObjectList)
        {
            _hitObjectDic.Remove(obj);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        //만약 피격당한 object dic에 등록되있다면? 리턴
        if (_hitObjectDic.ContainsKey(other.gameObject))
            return;

        if (other.TryGetComponent(out IHp iHp))
        {
            AttackTarget(iHp, _damageValue);
            _hitObjectDic.Add(other.gameObject, _damageInterval);
        }
    }

}
