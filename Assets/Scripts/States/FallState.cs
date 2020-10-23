using NodeCanvas;

public class FallState : State
{
    private readonly Character2D character;
    private readonly ICommandAdventurer command;

    private bool allowAirControl;
    public  bool AllowAirControl => allowAirControl;

    public FallState(Character2D character, ICommandAdventurer command, bool allowAirControl = false)
    {
        this.character = character;
        this.command   = command;
        
        this.allowAirControl = allowAirControl;
    }

    public override void OnEnter()
    {
        character.PlayFallAnimation();
    }

    public override void OnFixedUpdate()
    {
        if (allowAirControl) 
            character.AirControl(command.MoveX.Value);
    }

    public override void OnExit()
    {
        character.Stop();
    }
}