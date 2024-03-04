using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    void Update()
    {
        if (canDealDamage)
        {

          
            RaycastHit hit;

            int layerMask = 1 << 11;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {

                GameObject obj = hasDealtDamage.Find(x => x == hit.transform.gameObject);

                if (obj == null)
                {
                    if (hit.transform.gameObject.TryGetComponent(out IHp iHp))
                    {
                        iHp.DepleteHp(gameObject, weaponDamage);
                        hasDealtDamage.Add(hit.transform.gameObject);
                    }
                }
                //if (hit.transform.TryGetComponent(out Enemy enemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
                //{
                //    hasDealtDamage.Add(hit.transform.gameObject);
                //}

                //if (!hasDealtDamage.Contains(hit.transform.gameObject))
                //{
                //    Debug.Log("damage");
                //    hasDealtDamage.Add(hit.transform.gameObject);
                //}
            }
        }
    }
    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();
    }
    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
