using Node;

public class DrawSwordState : DurableState
{
    private readonly Character2D character;

    public DrawSwordState(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
        
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayDrawSwordAnimation();
        character.DrawWeapon();
    }
}
