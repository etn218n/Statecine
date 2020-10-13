using Node;

public class RollState : DurableState
{
    private readonly Character2D character;

    private float defaultGravityScale;

    public RollState(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayRollAnimation();
        character.Roll();
        character.InAction = true;

        defaultGravityScale = character.RB2D.gravityScale;
        character.RB2D.gravityScale = 1;
    }

    public override void OnExit()
    {
        character.InAction = false;
        character.RB2D.gravityScale = defaultGravityScale;
    }
}