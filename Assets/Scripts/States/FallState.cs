using Node;

public class FallState : State
{
    private readonly Character2D character;
    private readonly PlayerInput input;

    private bool allowAirControl;
    public  bool AllowAirControl => allowAirControl;

    public FallState(Character2D character, PlayerInput input, bool allowAirControl = false)
    {
        this.character = character;
        this.input     = input;
        
        this.allowAirControl = allowAirControl;
    }

    public override void OnEnter()
    {
        character.PlayFallAnimation();
    }

    public override void OnUpdate()
    {
        if (allowAirControl) 
            character.AirControl(input.Horizontal);
    }

    public override void OnExit()
    {
        character.Stop();
    }
}