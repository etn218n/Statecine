using Node;

public class GetUpState : DurableState
{
    private readonly Character2D character;

    public GetUpState(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayGetUpAnimation();
    }
}