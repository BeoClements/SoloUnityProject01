using UnityEditor;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float CurrentHealth;
    public float StartingHealth;
    public float DamageTakenAmount;
    public bool HasTakenDamage;
    public RectTransform HealthFiller;
    public float HealthFillerMax;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHealth = StartingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        TakeDamage();
        ClampHealth();
        UpdateUI();
    }
    void TakeDamage()
    {
        if (HasTakenDamage)
        {
            CurrentHealth -= DamageTakenAmount;
            DamageTakenAmount = 0;
            HasTakenDamage = false;
        }
    }
    void UpdateUI()
    {
        HealthFiller.transform.localScale = new Vector2((CurrentHealth / StartingHealth) * HealthFillerMax, HealthFiller.transform.localScale.y);
    }
    void ClampHealth()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, StartingHealth);
    }
}
