using UnityEngine;

public class Dash : MonoBehaviour
{
    public PlayerEnergy playerEnergy;
    public Movement2 PlayerMovement;
    public bool AttemptDash;
    public bool CanDash;
    public float EnergyUsed;
    public float DashJumpStregth;
    public float DashStrength;
    public float AirModifier;
    public AudioSource audioSource;
    public AudioClip DashSFX;
    public float DashSoundVolume = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AttemptDash = Input.GetKey(KeyCode.LeftShift);
    }
    void FixedUpdate()
    {
        if (PlayerMovement.OnGround)
        {
            CanDash = true;
        }
        if (AttemptDash && CanDash && playerEnergy.CurrentEnergy > EnergyUsed)
        {
            audioSource.PlayOneShot(DashSFX, DashSoundVolume);
            if (PlayerMovement.OnGround)
            {
                playerEnergy.TimeSinceLastUse = 0;
                playerEnergy.CurrentEnergy -= EnergyUsed;
                float SavedJumpStregth = PlayerMovement.JumpStrength;
                PlayerMovement.rb.linearVelocity = PlayerMovement.rb.linearVelocity * DashStrength;
                PlayerMovement.JumpStrength = DashJumpStregth;
                PlayerMovement.AttemptJump = true;
                CanDash = false;
                PlayerMovement.JumpStrength = SavedJumpStregth;
            }
            else
            {
                playerEnergy.TimeSinceLastUse = 0;
                playerEnergy.CurrentEnergy -= EnergyUsed;
                PlayerMovement.rb.linearVelocity = PlayerMovement.NormalizedInputVector * (DashStrength * AirModifier);
                CanDash = false;
            }
        }
    }
}
