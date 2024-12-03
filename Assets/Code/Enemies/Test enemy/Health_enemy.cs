using System;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Health_enemy : MonoBehaviour
{
    public float DamageTakenAmount;
    public bool TakenDamage;
    public float Health;
    public float StartingHealth = 1000;
    public TextMeshPro HealthText;
    public bool immortal = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Health = StartingHealth;
        DamageTakenAmount = 0;
        TakenDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        HealthText.text = Convert.ToString(MathF.Round(Health));
    }
    void FixedUpdate()
    {
        if (immortal)
        {
            TakenDamage = false;
            DamageTakenAmount = 0;
        }
        if (TakenDamage)
        {
            Health -= DamageTakenAmount;
            //Debug.Log("Damage Taken" + Convert.ToInt32(DamageTakenAmount) + "Current Health" + Convert.ToInt32(Health));
            DamageTakenAmount = 0;
            TakenDamage = false;
        }
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
