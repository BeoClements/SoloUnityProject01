using TMPro;
using UnityEngine;

public class Boss1MoveAndAttack : MonoBehaviour
{
    public Health_enemy health_Enemy;
    public TextMeshPro HealthText;
    public Rigidbody rb;
    public bool IAreAwake;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IAreAwake)
        {
            Vector3 Randomforce = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
            Randomforce *= 10;
            rb.AddForce(Randomforce, ForceMode.Impulse);
        }
    }

    public void WakeUp()
    {
        health_Enemy.immortal = false;
        HealthText.enabled = true;
        IAreAwake = true;
    }
}
