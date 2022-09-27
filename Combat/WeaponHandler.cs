using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    //[SerializeField] private WeaponDamage weaponDamage;
    [field: SerializeField] public Weapon Weapon;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform weaponAttachment; // weapon placement in the back

    private bool weaponEquipped = false;
    private GameObject weaponAttached;

    private void Start()
    {
        weaponAttached = Instantiate(weaponPrefab, weaponAttachment);
    }

    // methods are being called in the animation (animation events)
    private void EnableWeapon()
    {
        if (weaponPrefab.TryGetComponent(out CapsuleCollider collider))
        {
            collider.enabled = true;
            Weapon.AlreadyCollidedWith.Clear();
        }
    }

    private void DisableWeapon()
    {

        if (weaponPrefab.TryGetComponent(out CapsuleCollider collider))
        {
            collider.enabled = false;
        }
    }

    private void WeaponSwitch()
    {
        if (weaponEquipped)
        {
            weaponAttached.transform.SetParent(rightHandTransform);
        }
        else
        {
            weaponAttached.transform.SetParent(weaponAttachment);
        }

        weaponAttached.transform.localPosition = Vector3.zero;
        weaponAttached.transform.localRotation = Quaternion.identity;
    }

    public void SetWeaponEquip(bool isEquipped)
    {
        weaponEquipped = isEquipped;
    }
}
