using NodeCanvas;

public class JumpState : DurableState
{
    private readonly Character2D character;
    private readonly ICommandAdventurer command;
    
    private bool allowAirControl;
    public  bool AllowAirControl => allowAirControl;

    public JumpState(Character2D character, ICommandAdventurer command, float duration, bool allowAirControl = false) : base(duration)
    {
        this.character = character;
        this.command   = command;
        
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
            character.AirControl(command.MoveX.Value);
    }
}