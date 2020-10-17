using Node;

public class CrouchWalkState : State
{
    private readonly Character2D character;
    private readonly ICommandAdventurer command;

    public CrouchWalkState(Character2D character, ICommandAdventurer command)
    {
        this.character = character;
        this.command   = command;
    }
        
    public override void OnEnter()
    {
        character.PlayCrouchWalkAnimation();
        character.UpperBodyCollider.isTrigger = true;
    }

    public override void OnFixedUpdate()
    {
        character.CrouchWalk(command.MoveX.Value);
    }

    public override void OnExit()
    {
        character.UpperBodyCollider.isTrigger = false;
    }
}