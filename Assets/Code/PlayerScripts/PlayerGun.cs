using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

public class PlayerGun : MonoBehaviour
{
    public Transform Camera1;
    public float DamagePerSecond;
    public RaycastHit hit;
    public MeshRenderer BulletEfects;
    public float EnergyUsePerSecond;
    public PlayerEnergy EnergyScript;
    public AudioSource LaserSFX;
    public float LaserVolume = 1;
    public float LaserPitch = 1;
    public GameObject LaserLight;
    public Transform LaserLightTransform;
    public float LaserlightDistanceFromHit;
    public LayerMask HitLM;

    private bool Attack = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Attack = false;
        BulletEfects.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        InputHandling();
        ActionAttack();
    }

    void FixedUpdate()
    {

    }
    public void InputHandling()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Attack = false;
        }
    }
    public void ActionAttack()
    {
        if (Attack && EnergyScript.CurrentEnergy > 1)
        {
            EnergyScript.TimeSinceLastUse = 0;
            float DamageDealt;
            DamageDealt = MathF.Min(DamagePerSecond * Time.deltaTime, DamagePerSecond * Time.deltaTime * (EnergyScript.CurrentEnergy / EnergyUsePerSecond));
            BulletEfects.enabled = true;
            if (LaserSFX.volume == 0)
            {
                LaserSFX.Play();
            }
            LaserSFX.volume = LaserVolume;
            LaserSFX.pitch = LaserPitch;
            // Perform the raycast
            Debug.DrawRay(transform.position, Camera1.forward, Color.red);
            if (Physics.Raycast(transform.position, Camera1.forward, out hit, Mathf.Infinity, HitLM))
            {
                LaserLight.SetActive(true);
                Vector3 direction = hit.normal;
                LaserLightTransform.position = hit.point;
                LaserLightTransform.position += direction * LaserlightDistanceFromHit;
                // Check if the hit object has the Health_enemy component
                Health_enemy healthEnemy = hit.transform.GetComponent<Health_enemy>();

                // If it does, decrease the Health by 1
                if (healthEnemy != null)
                {
                    healthEnemy.DamageTakenAmount += DamageDealt;
                    healthEnemy.TakenDamage = true;
                }
            }
            else
            {
                LaserLight.SetActive(false);
            }
            EnergyScript.CurrentEnergy -= EnergyUsePerSecond * Time.deltaTime;
        }
        else
        {
            LaserSFX.volume = 0;
            LaserSFX.pitch = 0;
            BulletEfects.enabled = false;
            LaserLight.SetActive(false);
        }
    }
}
