using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthDisplay;
    [SerializeField] private TextMeshProUGUI staminaDisplay;

    private GameObject player;
    private Health health;
    private Stamina stamina;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = player.GetComponent<Health>();
        stamina = player.GetComponent<Stamina>();
    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.text = health.GetValue.ToString();
        staminaDisplay.text = stamina.GetValue.ToString("0.00");
    }
}
