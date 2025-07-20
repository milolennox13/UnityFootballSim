using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool inGoal = false;

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Reset()
    {
        transform.position = Vector2.zero; // Reset ball to center
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop movement
    }

    void Start()
    {

        if (GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogError("Rigidbody2D is missing from this GameObject!");
        }
    }

    void Update()
    {
        
    }

}