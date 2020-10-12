using Node;

public class IdleState : State
{
    private readonly Character2D character;

    public IdleState(Character2D character)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        character.Stop();
        character.PlayIdleAnimation();
    }
}
