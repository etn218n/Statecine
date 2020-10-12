using Node;

public class CrouchWalkState : State
{
    private readonly Character2D character;
    private readonly PlayerInput input;

    public CrouchWalkState(Character2D character, PlayerInput input)
    {
        this.character = character;
        this.input     = input;
    }
        
    public override void OnEnter()
    {
        character.PlayCrouchWalkAnimation();
    }

    public override void OnUpdate()
    {
        character.CrouchWalk(input.Horizontal);
    }
}