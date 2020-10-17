using UnityEngine;

public class PlayerCommandAdventurer : MonoBehaviour, ICommandAdventurer
{
    public ICharacterAction<float> MoveX { get; } = new HorizontalKeyAction();
    
    public ICharacterAction Jump { get; } = new KeyAction(KeyCode.Space, KeyCode.UpArrow);
    public ICharacterAction Roll { get; } = new KeyAction(KeyCode.C);
    public ICharacterAction Crouch { get; } = new KeyAction(KeyCode.LeftControl, KeyCode.DownArrow);
    public ICharacterAction Sprint { get; } = new KeyAction(KeyCode.LeftShift);
    public ICharacterAction KnockDown  { get; } = new KeyAction(KeyCode.K);
    public ICharacterAction DrawWeapon { get; } = new KeyAction(KeyCode.V);
    public ICharacterAction SheathWeapon { get; } = new KeyAction(KeyCode.V);
    public ICharacterAction PrimaryAttack   { get; } = new KeyAction(KeyCode.Z);
    public ICharacterAction SecondaryAttack { get; } = new KeyAction(KeyCode.X);

    private void Update()
    {
        MoveX.Value = Input.GetAxisRaw("Horizontal");
    }
}

public class HorizontalKeyAction : ICharacterAction<float>
{
    public bool Start   { get => value != 0; }
    public bool Perform { get => value != 0; }
    public bool Cancel  { get => value == 0; }

    public float Value
    {
        get => value;
        set => this.value = value;
    }

    private float value;
}