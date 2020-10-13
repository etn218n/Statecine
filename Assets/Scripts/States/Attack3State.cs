using Node;

public class Attack3State : DurableState
{
    private readonly Character2D character;

    public Attack3State(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayAttack3Animation();
        character.InAction = true;
    }

    public override void OnExit()
    {
        character.InAction = false;
    }
}