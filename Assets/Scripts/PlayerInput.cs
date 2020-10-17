using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal
    {
        get => Input.GetAxisRaw("Horizontal");
    }
    public float Vertical
    {
        get => Input.GetAxisRaw("Vertical");
    }
    public bool MoveX => (Horizontal != 0);
    public bool MoveY => (Vertical != 0);
    public bool Move => (Horizontal != 0) || (Vertical != 0);

    public KeyAction Jump = new KeyAction(KeyCode.Space, KeyCode.UpArrow);
    public KeyAction Roll = new KeyAction(KeyCode.C);
    public KeyAction Crouch = new KeyAction(KeyCode.LeftControl, KeyCode.DownArrow);
    public KeyAction Sprint = new KeyAction(KeyCode.LeftShift);
    public KeyAction KnockDown = new KeyAction(KeyCode.K);
    public KeyAction DrawWeapon = new KeyAction(KeyCode.V);
    public KeyAction SheathWeapon = new KeyAction(KeyCode.V);
    public KeyAction PrimaryAttack = new KeyAction(KeyCode.Z);
    public KeyAction SecondaryAttack = new KeyAction(KeyCode.X);

    private void Update()
    {
        //Horizontal = Input.GetAxisRaw("Horizontal");
        //Vertical   = Input.GetAxisRaw("Vertical");
    }
}
