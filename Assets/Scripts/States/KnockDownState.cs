using NodeCanvas;

public class KnockDownState : DurableState
{
    private readonly Character2D character;

    public KnockDownState(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayKnockDownAnimation();
        character.GetKnockDown();
        character.InAction = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        
        character.InAction = false;
    }
}