using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool IsColliding => colliderCount > 0;
    
    private int colliderCount = 0;
    
    public LayerMask AllowedLayers = default(LayerMask);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & AllowedLayers) != 0)
            colliderCount++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & AllowedLayers) != 0)
            colliderCount--;
    }
}
