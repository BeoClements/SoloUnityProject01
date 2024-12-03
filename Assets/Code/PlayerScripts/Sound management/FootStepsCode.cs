using UnityEngine;

public class FootStepsCode : MonoBehaviour
{
    public bool IsWalking;
    private float TimeSinceLastStep;
    public float StepRate;
    public AudioClip[] audioClipsFootsteps;
    public AudioClip[] audioClipsLandings;
    private AudioSource audioSource;
    public float LandingsVolume = 1;
    public float FootStepsVolume = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeSinceLastStep > StepRate && IsWalking)
        {
            TimeSinceLastStep = 0;
            PlayRandomClip();
        }
        else
        {
            TimeSinceLastStep += Time.deltaTime;
        }
    }
    void PlayRandomClip()
    {
        int index = Random.Range(0, audioClipsFootsteps.Length);
        audioSource.PlayOneShot(audioClipsFootsteps[index], FootStepsVolume);
    }
    public void OnLand(float LandedSpeed)
    {
        int index = Random.Range(0, audioClipsFootsteps.Length);
        audioSource.PlayOneShot(audioClipsLandings[index], LandingsVolume * -LandedSpeed);
    }
}
