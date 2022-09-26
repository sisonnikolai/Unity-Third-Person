using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    [field: SerializeField] public Weapon Weapon { get; private set; }

    public void EnableWeapon()
    {
        Weapon.EnableWeapon();
    }

    public void DisableWeapon()
    {
        Weapon.DisableWeapon();
    }
}
