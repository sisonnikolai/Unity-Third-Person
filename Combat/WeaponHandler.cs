using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    //[SerializeField] private WeaponDamage weaponDamage;
    [SerializeField] private Collider myCollider;
    [SerializeField] public Weapon weapon;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform backAttachment = null; // weapon placement in the back
    [SerializeField] private Transform sideAttachment = null; // weapon placement in the side (eg. daggers)

    private bool weaponEquipped = false;
    private GameObject weaponAttached;
    private const string weaponName = "Weapon";

    private void Start()
    {
        //if (weaponAttached == null) { return; }
        //weaponAttached = Instantiate(weaponPrefab, backAttachment);
        //weaponAttached.name = weaponName;
        weaponAttached = weaponPrefab;
    }

    // methods are being called in the animation (animation events)
    private void EnableWeapon()
    {
        if (weaponAttached == null) { return; }
        if (weaponAttached.TryGetComponent(out CapsuleCollider collider))
        {
            collider.enabled = true;
            weapon.AlreadyCollidedWith.Clear();
        }
    }

    private void DisableWeapon()
    {

        if (weaponAttached == null) { return; }
        if (weaponAttached.TryGetComponent(out CapsuleCollider collider))
        {
            collider.enabled = false;
        }
    }

    private void WeaponSwitch()
    {
        if (weaponAttached == null) { return; }
        if (weaponEquipped)
        {
            weaponAttached.transform.SetParent(rightHandTransform);
        }
        else
        {
            weaponAttached.transform.SetParent(backAttachment);
        }

        weaponAttached.transform.localPosition = Vector3.zero;
        weaponAttached.transform.localRotation = Quaternion.identity;
    }

    public void SetWeaponEquip(bool isEquipped)
    {
        weaponEquipped = isEquipped;
    }

    public void AttachWeapon(GameObject weaponPrefab, string weaponPlacement)
    {
        // TODO: destroy old weapon
        this.weaponPrefab = weaponPrefab;
        //weaponEquipped = true;

        if (weaponPlacement == "Back")
        {
            weaponAttached = Instantiate(this.weaponPrefab, backAttachment);
        }
        else if (weaponPlacement == "Side")
        {
            weaponAttached = Instantiate(this.weaponPrefab, sideAttachment);
        }
        weaponAttached.name = weaponName;
        this.weapon = weaponAttached.GetComponent<Weapon>();

        this.weapon.SetCollider(myCollider);
    }

    public bool HasWeapon()
    {
        return weaponPrefab != null;
    }

    public bool IsWeaponEquipped()
    {
        return weaponEquipped;
    }
}
