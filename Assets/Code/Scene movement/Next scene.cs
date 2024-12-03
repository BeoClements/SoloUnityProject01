using UnityEngine;
using UnityEngine.SceneManagement;

public class Nextscene : MonoBehaviour
{
    private Collider col;
    public
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        SceneManager.LoadScene(1);
    }
}
