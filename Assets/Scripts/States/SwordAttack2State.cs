using NodeCanvas;

public class SwordAttack2State : DurableState
{
    private readonly Character2D character;

    public SwordAttack2State(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlaySwordAttack2Animation();
        character.InAction = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        
        character.InAction = false;
    }
}