using NodeCanvas;

public class Punch1State : DurableState
{
    private readonly Character2D character;

    public Punch1State(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayPunch1Animation();
        character.InAction = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        
        character.InAction = false;
    }
}