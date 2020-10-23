using NodeCanvas;

public class SheathSwordState : DurableState
{
    private readonly Character2D character;

    public SheathSwordState(Character2D character, float duration) : base(duration)
    {
        this.character = character;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlaySheathSwordAnimation();
        character.SheathWeapon();
    }
}