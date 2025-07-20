using UnityEngine;

public class TopDownPhysics : MonoBehaviour
{
    public Vector2 velocity;  // Velocity of the object
    public float friction = 1.5f; // Custom friction to slow down the object
    void Start()
    {
        velocity = new Vector2(0f, 0f); // Initialise velocity
    }
    void FixedUpdate()
    {
        // Move the object based on its velocity
        transform.position += (Vector3)velocity * Time.deltaTime;

        // Apply friction
        ApplyFriction();
    }

    void ApplyFriction()
    {
        velocity.x *= (1 - friction * Time.deltaTime);
        velocity.y *= (1 - friction * Time.deltaTime);
    }
}