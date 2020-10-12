using Node;

public class RunState : State
{
    private readonly Character2D character;
    private readonly PlayerInput input;

    public RunState(Character2D character, PlayerInput input)
    {
        this.character = character;
        this.input     = input;
    }
        
    public override void OnEnter()
    {
        character.PlayRunAnimation();
    }

    public override void OnUpdate()
    {
        character.Run(input.Horizontal);
    }
}