using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 50f;
    private float stamina;
    private float recoveryRate = 2.5f;

    void Start()
    {
        stamina = maxStamina;
    }

    void Update()
    {
        if (stamina < maxStamina)
        {
            stamina += (Time.deltaTime * recoveryRate);
        }
    }

    public float GetValue => stamina;

    public void Reduce(float usage)
    {
        stamina -= usage;
    }
}
