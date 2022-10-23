using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private Collider myCollider;
    [SerializeField] public Weapon weapon; // TODO: remove SerializeField. Get weapon ccmponent from the weaponPrefab instead
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform backAttachment = null; // weapon placement in the back
    [SerializeField] private Transform sideAttachment = null; // weapon placement in the side (eg. daggers)

    private bool weaponEquipped = false;
    private GameObject weaponAttached;
    private const string weaponName = "Weapon";

    private void Start()
    {
        if (weaponPrefab == null) { return; }

        // instantiates a weapon by default.
        weaponAttached = Instantiate(weaponPrefab, backAttachment);
        weaponAttached.name = weaponName;
        weapon = weaponAttached.GetComponent<Weapon>();
        this.weapon.SetCollider(myCollider);
        weaponEquipped = true;
    }

    #region Animation Events
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
    #endregion

    public void SetWeaponEquip(bool isEquipped)
    {
        weaponEquipped = isEquipped;
    }

    public void AttachWeapon(GameObject weaponPrefab, string weaponPlacement)
    {
        // TODO: destroy old weapon
        this.weaponPrefab = weaponPrefab;

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
