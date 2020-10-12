using Node;

public class SprintState : State
{
    private readonly Character2D character;
    private readonly PlayerInput input;

    public SprintState(Character2D character, PlayerInput input)
    {
        this.character = character;
        this.input     = input;
    }
        
    public override void OnEnter()
    {
        character.PlaySprintAnimation();
    }

    public override void OnUpdate()
    {
        character.Sprint(input.Horizontal);
    }
}