using Node;

public class AirAttack1State : DurableState
{
    private readonly Character2D character;

    public AirAttack1State(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayAirAttack1Animation();
        character.InAction = true;
    }

    public override void OnExit()
    {
        character.InAction = false;
    }
}