using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossMeeleAttack : BossAttack
{
    private List<GameObject> _hitObjects = new List<GameObject>();


    private void Start()
    {
        gameObject.SetActive(false);
    }


    public override void StartAttack()
    {
        _hitObjects.Clear();
        gameObject.SetActive(true);
    }


    public override void EndAttack()
    {
        gameObject.SetActive(false);
    }


    private void OnTriggerStay(Collider other)
    {
        GameObject obj = _hitObjects.Find(x => x == other.gameObject);

        if (obj == null)
        {
            if(other.TryGetComponent(out IHp iHp))
            {
                iHp.DepleteHp("boss" , _damage);
                _hitObjects.Add(other.gameObject);
            }
        }
    }
}
