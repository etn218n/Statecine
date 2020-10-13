using Node;

public class FallState : State
{
    private readonly Character2D character;
    private readonly PlayerInput input;

    public FallState(Character2D character, PlayerInput input)
    {
        this.character = character;
        this.input     = input;
    }

    public override void OnEnter()
    {
        character.PlayFallAnimation();
    }

    public override void OnUpdate()
    {
        character.Run(input.Horizontal);
    }
}