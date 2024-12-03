using UnityEngine;

public class WakeUpBoss : MonoBehaviour
{
    public Boss1MoveAndAttack BossScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        BossScript.WakeUp();
    }
}
