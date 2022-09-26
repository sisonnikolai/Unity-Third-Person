using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private int attackDamage = 0;
    private float attackKnockback = 1f;
    public List<Collider> AlreadyCollidedWith { get; private set; } = new List<Collider>();

    private void OnEnable()
    {
        //AlreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (myCollider.CompareTag("Enemy") && other.CompareTag("Enemy")) { return; } // prevent enemies from hitting each other

        if (other == myCollider) { return; }

        if (AlreadyCollidedWith.Contains(other)) { return; }

        AlreadyCollidedWith.Add(other);

        if (other.TryGetComponent(out Health health))
        {
            health.DealDamange(attackDamage);
        }

        if (other.TryGetComponent(out ForceReceiver forceReceiver))
        {
            Vector3 forceDirection = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(forceDirection * attackKnockback);
        }
    }

    public void SetAttack(int damage, float knockback)
    {
        attackDamage = damage;
        attackKnockback = knockback;
    }
}
