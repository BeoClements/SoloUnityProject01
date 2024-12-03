using UnityEngine;

public class RandomMover : MonoBehaviour
{
    public Vector3 areaSize = new Vector3(10f, 10f, 10f); // Size of the area in which the object will move
    public float speed = 5f; // Movement speed of the object

    private Vector3 targetPosition;

    void Start()
    {
        SetNewTargetPosition();
    }

    void Update()
    {
        MoveToTarget();
    }

    void SetNewTargetPosition()
    {
        // Generate a random target position within the defined area
        targetPosition = new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            Random.Range(-areaSize.y / 2, areaSize.y / 2),
            Random.Range(-areaSize.z / 2, areaSize.z / 2)
        );
    }

    void MoveToTarget()
    {
        // Move the object towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // If the object reaches the target position, set a new random target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }

    void OnDrawGizmos()
    {
        // Draw a wireframe cube to represent the movement area
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
