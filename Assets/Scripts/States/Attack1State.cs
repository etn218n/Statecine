using Node;

public class Attack1State : DurableState
{
    private readonly Character2D character;

    public Attack1State(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayAttack1Animation();
        character.InAction = true;
    }

    public override void OnExit()
    {
        character.InAction = false;
    }
}