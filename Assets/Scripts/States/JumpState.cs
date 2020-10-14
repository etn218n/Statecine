using Node;

public class JumpState : DurableState
{
    private readonly Character2D character;
    private readonly PlayerInput input;
    
    private bool allowAirControl;
    public  bool AllowAirControl => allowAirControl;

    public JumpState(Character2D character, PlayerInput input, float duration, bool allowAirControl = false) : base(duration)
    {
        this.character = character;
        this.input     = input;
        
        this.allowAirControl = allowAirControl;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayJumpAnimation();
        character.Jump();
    }

    public override void OnFixedUpdate()
    {
        if (allowAirControl) 
            character.AirControl(input.Horizontal);
    }
}