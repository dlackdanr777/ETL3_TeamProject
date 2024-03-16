using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Projectile : MonoBehaviour, IAttack
{
    //������ ���� ����(ex. 0.1�� ��� ��� �ִ� ��� ��� 0.1�ʸ��� ������)
    private float _damageInterval = 1;

    //Ÿ�� ������
    private float _damageValue;
    public float DamageValue => _damageValue;

    private object _subject;

    private Dictionary<GameObject, float> _hitObjectDic = new Dictionary<GameObject, float>();

    //Dic���� ������ Obj�� �����صδ� ����Ʈ
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

        //TODO: ���� ���� (ToList�� ����� �޸𸮸� ���� ����� �� ����)
        foreach (GameObject obj in _hitObjectDic.Keys.ToList())
        {
            // �ƴ� ��� Time.deltaTime��ŭ�� ����.
            _hitObjectDic[obj] = _hitObjectDic[obj] - Time.deltaTime;

            // ���� _damageInterval �ð��� �����ٸ� ����Ʈ�� �߰�
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
        //���� �ǰݴ��� object dic�� ��ϵ��ִٸ�? ����
        if (_hitObjectDic.ContainsKey(other.gameObject))
            return;

        if (other.TryGetComponent(out IHp iHp))
        {
            AttackTarget(iHp, _damageValue);
            _hitObjectDic.Add(other.gameObject, _damageInterval);
        }
    }

}
