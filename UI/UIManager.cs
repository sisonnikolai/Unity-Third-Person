using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthDisplay;

    private GameObject player;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = player.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.text = health.GetValue.ToString();
    }
}
