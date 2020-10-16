using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical   { get; private set; }
    public bool HasValueX => (Horizontal != 0);
    public bool HasValueY => (Vertical != 0);
    public bool HasValue => (Horizontal != 0) || (Vertical != 0);

    public InputAction Jump = new InputAction(KeyCode.Space, KeyCode.UpArrow);
    public InputAction Roll = new InputAction(KeyCode.C);
    public InputAction Crouch = new InputAction(KeyCode.LeftControl, KeyCode.DownArrow);
    public InputAction Sprint = new InputAction(KeyCode.LeftShift);
    public InputAction KnockDown = new InputAction(KeyCode.K);
    public InputAction DrawWeapon = new InputAction(KeyCode.V);
    public InputAction SheathWeapon = new InputAction(KeyCode.V);
    public InputAction PrimaryAttack = new InputAction(KeyCode.Z);
    public InputAction SecondaryAttack = new InputAction(KeyCode.X);

    private void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical   = Input.GetAxisRaw("Vertical");
    }
}
