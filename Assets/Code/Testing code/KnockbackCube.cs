using UnityEngine;

public class KnockbackCube : MonoBehaviour
{
    public float ExplosionForce;
    public float ExplosionRadius;
    public float UpwardsModifier;
    public Rigidbody playerRB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.T))
        {
            playerRB.AddExplosionForce(10, transform.position, ExplosionRadius, UpwardsModifier, ForceMode.Impulse);
            Debug.Log("Knockback cube has knocked back");
        }
    }
}
