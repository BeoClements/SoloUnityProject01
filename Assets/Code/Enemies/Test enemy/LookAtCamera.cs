using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera M_camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        M_camera = Camera.main;
        transform.LookAt(transform.position + M_camera.transform.rotation * Vector3.forward);
    }
}
