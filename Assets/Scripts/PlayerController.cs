using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private TopDownPhysics topdownPhysics;
    void Start()
    {
        // Find the TopDownPhysics component on the same GameObject
        topdownPhysics = GetComponent<TopDownPhysics>();

        if (topdownPhysics == null)
        {
            Debug.LogError("topdownPhysics is missing from this GameObject!");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Checks if Spacebar is pressed
        {
            if (topdownPhysics != null)
            {
            topdownPhysics.velocity += new Vector2(5, 0);
            }
        }
    }
}