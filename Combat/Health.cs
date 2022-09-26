using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;
    private bool isInvulnerable = false;
    public Action OnTakeDamage;
    public Action OnDie;

    void Start()
    {
        health = maxHealth;
    }

    public bool IsDead => health == 0;

    public void DealDamange(int damage)
    {
        if (health == 0) { return; }
        if (isInvulnerable) { return; }
        health = Mathf.Max(health - damage, 0);
        OnTakeDamage?.Invoke();

        if (health <= 0)
        {
            OnDie?.Invoke();
        }

        Debug.Log($"{gameObject.name}: {health}");
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }
}
