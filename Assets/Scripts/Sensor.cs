using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool IsColliding => colliderCount > 0;
    
    private int colliderCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        colliderCount++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        colliderCount--;
    }
}
