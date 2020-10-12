using Node;

public class SwordRunState : State
{
    private readonly Character2D character;
    private readonly PlayerInput input;

    public SwordRunState(Character2D character, PlayerInput input)
    {
        this.character = character;
        this.input     = input;
    }
        
    public override void OnEnter()
    {
        character.PlaySwordRunAnimation();
    }

    public override void OnUpdate()
    {
        character.Run(input.Horizontal);
    }
}