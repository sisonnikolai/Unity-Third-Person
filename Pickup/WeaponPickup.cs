using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab = null;
    [SerializeField] private Weapon weapon = null;
    [SerializeField] private WeaponPlacement weaponPlacement;

    enum WeaponPlacement
    {
        Back,
        Side
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<WeaponHandler>().AttachWeapon(weaponPrefab, weaponPlacement.ToString());
            Destroy(this.gameObject);
        }
    }
}
