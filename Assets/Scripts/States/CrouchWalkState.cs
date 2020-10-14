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
        character.UpperBodyCollider.isTrigger = true;
    }

    public override void OnFixedUpdate()
    {
        character.CrouchWalk(input.Horizontal);
    }

    public override void OnExit()
    {
        character.UpperBodyCollider.isTrigger = false;
    }
}