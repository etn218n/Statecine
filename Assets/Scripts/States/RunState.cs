using Node;

public class RunState : State
{
    private readonly Character2D character;
    private readonly ICommandAdventurer command;

    public RunState(Character2D character, ICommandAdventurer command)
    {
        this.character = character;
        this.command   = command;
    }
        
    public override void OnEnter()
    {
        character.PlayRunAnimation();
    }

    public override void OnFixedUpdate()
    {
        character.Run(command.MoveX.Value);
    }
}