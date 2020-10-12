using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical   { get; private set; }
    public bool HasValue => (Horizontal != 0);

    private void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical   = Input.GetAxisRaw("Vertical");
    }
}
