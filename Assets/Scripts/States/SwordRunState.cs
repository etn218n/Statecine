using Node;

public class SwordRunState : State
{
    private readonly Character2D character;
    private readonly ICommandAdventurer command;

    public SwordRunState(Character2D character, ICommandAdventurer command)
    {
        this.character = character;
        this.command   = command;
    }
        
    public override void OnEnter()
    {
        character.PlaySwordRunAnimation();
    }

    public override void OnFixedUpdate()
    {
        character.Run(command.MoveX.Value);
    }
}