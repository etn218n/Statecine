using System;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public event Action Collided;
    public event Action NotCollided;
    
    public bool IsColliding => colliderCount > 0;
    
    private int colliderCount = 0;
    
    public LayerMask AllowedLayers = default(LayerMask);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & AllowedLayers) != 0)
        {
            colliderCount++;
            
            if (colliderCount == 1)
                Collided?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & AllowedLayers) != 0)
        {
            colliderCount--; 
            
            if (colliderCount == 0)
                NotCollided?.Invoke();
        }
    }
}
