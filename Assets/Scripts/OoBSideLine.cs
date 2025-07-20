using UnityEngine;

public class OoBSideLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) // Ensure the Ball has the "Ball" tag
        {
            other.GetComponent<Ball>().Reset();
        }
    }
}