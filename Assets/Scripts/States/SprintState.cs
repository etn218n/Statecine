using Node;

public class SprintState : State
{
    private readonly Character2D character;
    private readonly ICommandAdventurer command;

    public SprintState(Character2D character, ICommandAdventurer command)
    {
        this.character = character;
        this.command   = command;
    }
        
    public override void OnEnter()
    {
        character.PlaySprintAnimation();
    }

    public override void OnFixedUpdate()
    {
        character.Sprint(command.MoveX.Value);
    }
}