using Unity.VisualScripting;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public float CurrentEnergy;
    public float MaxEnergy = 100;
    public float EnergyRegenPerSecond = 40;
    public RectTransform EnergyFiller;
    public float EnergyFillerMax = 2.85f;
    public float TimeSinceLastUse;
    public float EnergyRegenResetTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentEnergy = MaxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        EnergyRegen();
        ClampEnergy();
        UpdateUI();
    }
    void EnergyRegen()
    {
        TimeSinceLastUse += Time.deltaTime;
        if (TimeSinceLastUse > EnergyRegenResetTime)
        {
            CurrentEnergy += EnergyRegenPerSecond * Time.deltaTime;
        }
    }
    void ClampEnergy()
    {
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
    }
    void UpdateUI()
    {
        EnergyFiller.transform.localScale = new Vector2(EnergyFiller.transform.localScale.x, (CurrentEnergy / MaxEnergy) * EnergyFillerMax);
    }
}
