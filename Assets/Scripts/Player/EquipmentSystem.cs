using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject weaponSheath;

    GameObject currentWeaponInhand;
    GameObject currentWeaponInhandSheath;

    private void Start()
    {
        currentWeaponInhandSheath= Instantiate(weapon,weaponSheath.transform);
    }

    public void DrawWeapon()
    {
        currentWeaponInhand=Instantiate(weapon,weaponHolder.transform);
        Destroy(currentWeaponInhandSheath);
    }


    public void SheathWeapon()
    {
        currentWeaponInhandSheath= Instantiate(weapon,weaponSheath.transform);
        Destroy(currentWeaponInhand);
    }
    
    public void StartDealDamage()
    {
        currentWeaponInhand.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }
    public void EndDealDamage()
    {
        currentWeaponInhand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }
}
