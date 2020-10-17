using Node;

public class AirSwordAttack1State : DurableState
{
    private readonly Character2D character;

    public AirSwordAttack1State(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayAirSwordAttack1Animation();
        character.InAction = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        
        character.InAction = false;
    }
}