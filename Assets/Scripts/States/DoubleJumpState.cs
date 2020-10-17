using Node;

public class DoubleJumpState : DurableState
{
    private readonly Character2D character;
    private readonly ICommandAdventurer command;
    
    private bool allowAirControl;
    public  bool AllowAirControl => allowAirControl;

    private bool isReady;
    public  bool IsReady => isReady;

    public DoubleJumpState(Character2D character, ICommandAdventurer command, float duration, bool allowAirControl = false) : base(duration)
    {
        this.character = character;
        this.command   = command;
        
        this.allowAirControl = allowAirControl;

        this.character.GroundSensor.Collided += OnGrounded;
    }

    private void OnGrounded()
    {
        isReady = true;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayJumpAnimation();
        character.DoubleJump();

        isReady = false;
    }

    public override void OnFixedUpdate()
    {
        if (allowAirControl) 
            character.AirControl(command.MoveX.Value);
    }
}