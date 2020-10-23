using NodeCanvas;

public class SwordIdleState : State
{
    private readonly Character2D character;

    public SwordIdleState(Character2D character)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        character.Stop();
        character.PlaySwordIdleAnimation();
    }
}