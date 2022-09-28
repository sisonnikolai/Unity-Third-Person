using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 50f;
    private float stamina;

    void Start()
    {
        stamina = maxStamina;
    }

    void Update()
    {
        if (stamina < maxStamina)
        {
            stamina += (Time.deltaTime * 2.5f);
        }

    }

    public float GetValue()
    {
        return stamina;
    }

    public void Reduce(float usage)
    {
        stamina -= usage;
    }
}
