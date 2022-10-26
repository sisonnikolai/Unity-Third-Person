using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 50f;

    private float stamina;
    private float combatRecoveryRate = 2.5f;
    private float idleRecoveryRate = 10f;
    private PlayerStateMachine stateMachine;

    void Start()
    {
        stamina = maxStamina;
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        if (stamina >= maxStamina) { return; }
        float recoveryRate = stateMachine.WeaponHandler.IsWeaponEquipped() ? combatRecoveryRate : idleRecoveryRate;
        stamina += (Time.deltaTime * recoveryRate);
    }

    public float GetValue => stamina;

    public void Reduce(float usage)
    {
        stamina -= usage;
    }
}
