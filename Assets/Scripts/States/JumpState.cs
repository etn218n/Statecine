using Node;

public class JumpState : DurableState
{
    private readonly Character2D character;
    private readonly PlayerInput input;

    public JumpState(Character2D character, PlayerInput input, float duration) : base(duration)
    {
        this.character = character;
        this.input     = input;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        
        character.PlayJumpAnimation();
        character.Jump();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        //character.MoveHorizontal(input.Horizontal);
    }
}