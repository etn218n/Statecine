using Node;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInput input = null;
    private Character2D character = null;
    
    private FSM fsm = new FSM();

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        character = GetComponent<Character2D>();
        
        var idle = new IdleState(character);
        var run  = new RunState(character, input);
        var jump = new JumpState(character, input, 0.01f, true);
        var doubleJump = new DoubleJumpState(character, input, 0.05f, true);
        var roll = new RollState(character, 0.45f);
        var fall = new FallState(character, input, true);
        var sprint = new SprintState(character, input);
        var getup  = new GetUpState(character, 0.5f);
        var crouch = new CrouchState(character);
        var crouchWalk = new CrouchWalkState(character, input);
        // var dead = new DeadState(character);
        var swordAttack1 = new SwordAttack1State(character, 0.35f);
        var swordAttack2 = new SwordAttack2State(character, 0.45f);
        var swordAttack3 = new SwordAttack3State(character, 0.45f);
        var airSwordAttack1 = new AirSwordAttack1State(character, 0.35f);
        var punch1 = new Punch1State(character, 0.4f);
        var punch2 = new Punch2State(character, 0.4f);
        var punch3 = new Punch3State(character, 0.4f);
        var drawSword   = new DrawSwordState(character, 0.4f);
        var sheathSword = new SheathSwordState(character, 0.4f);
        var swordIdle   = new SwordIdleState(character);
        var swordRun    = new SwordRunState(character, input);
        var knockdown   = new KnockDownState(character, 0.4f);
        
        fsm.AddTransition(idle, run,  () => input.HasValueX);
        fsm.AddTransition(idle, jump, () => input.Jump.Start);
        fsm.AddTransition(idle, roll, () => input.Roll.Perform);
        fsm.AddTransition(idle, punch1,    () => input.PrimaryAttack.Perform);
        fsm.AddTransition(idle, crouch,    () => input.Crouch.Start);
        fsm.AddTransition(idle, drawSword, () => input.DrawWeapon.Start);
        
        fsm.AddTransition(crouch, idle,       () => input.Crouch.Cancel && !character.IsEquipped);
        fsm.AddTransition(crouch, swordIdle,  () => input.Crouch.Cancel &&  character.IsEquipped);
        fsm.AddTransition(crouch, roll,       () => input.Roll.Perform);
        fsm.AddTransition(crouch, crouchWalk, () => input.HasValueX);

        fsm.AddTransition(crouchWalk, crouch,    () => !input.HasValueX);
        fsm.AddTransition(crouchWalk, idle,      () => input.Crouch.Cancel && !character.IsEquipped);
        fsm.AddTransition(crouchWalk, swordIdle, () => input.Crouch.Cancel &&  character.IsEquipped);
        fsm.AddTransition(crouchWalk, roll,      () => input.Roll.Perform);
        
        fsm.AddTransition(drawSword, swordIdle,   () => drawSword.IsDone);
        fsm.AddTransition(drawSword, sheathSword, () => input.SheathWeapon.Start);
        fsm.AddTransition(drawSword, swordRun,    () => drawSword.IsDone && input.HasValueX);
        
        fsm.AddTransition(swordIdle, swordRun,     () => input.HasValueX);
        fsm.AddTransition(swordIdle, jump,         () => input.Jump.Start);
        fsm.AddTransition(swordIdle, sheathSword,  () => input.SheathWeapon.Start);
        fsm.AddTransition(swordIdle, crouch,       () => input.Crouch.Start);
        fsm.AddTransition(swordIdle, swordAttack1, () => input.PrimaryAttack.Perform);
        fsm.AddTransition(swordIdle, roll,         () => input.Roll.Perform);
        
        fsm.AddTransition(swordRun, swordIdle,    () => !input.HasValueX);
        fsm.AddTransition(swordRun, jump,         () => input.Jump.Start);
        fsm.AddTransition(swordRun, sprint,       () => input.Sprint.Perform);
        fsm.AddTransition(swordRun, sheathSword,  () => input.SheathWeapon.Start);
        fsm.AddTransition(swordRun, crouch,       () => input.Crouch.Start);
        fsm.AddTransition(swordRun, roll,         () => input.Roll.Perform);
        fsm.AddTransition(swordRun, swordAttack1, () => input.PrimaryAttack.Perform);

        fsm.AddTransition(sheathSword, idle,      () => sheathSword.IsDone);
        fsm.AddTransition(sheathSword, drawSword, () => input.DrawWeapon.Start);
        fsm.AddTransition(sheathSword, run,       () => sheathSword.IsDone && input.HasValueX);
        
        fsm.AddTransition(run, idle,      () => !input.HasValueX);
        fsm.AddTransition(run, drawSword, () => input.DrawWeapon.Start);
        fsm.AddTransition(run, jump,      () => input.Jump.Start);
        fsm.AddTransition(run, sprint,    () => input.Sprint.Perform);
        fsm.AddTransition(run, crouch,    () => input.Crouch.Start);
        fsm.AddTransition(run, roll,      () => input.Roll.Perform);
        fsm.AddTransition(run, punch1,    () => input.PrimaryAttack.Perform);
        
        fsm.AddTransition(sprint, jump, () => input.Jump.Start);
        fsm.AddTransition(sprint, roll, () => input.Roll.Perform);
        fsm.AddTransition(sprint, idle, () => (input.Sprint.Cancel || !input.HasValueX) && !character.IsEquipped);
        fsm.AddTransition(sprint, swordIdle,    () => (input.Sprint.Cancel || !input.HasValueX) && character.IsEquipped);
        fsm.AddTransition(sprint, swordAttack1, () => input.PrimaryAttack.Perform && character.IsEquipped);
        
        //fsm.AddTransition(jump, FSM.PreviousState, () => character.IsGrounded && jump.IsDone);
        fsm.AddTransition(jump, roll, () => input.Roll.Perform);
        fsm.AddTransition(jump, punch3, () => input.PrimaryAttack.Perform && !character.IsEquipped);
        fsm.AddTransition(jump, doubleJump, () => input.Jump.Start && jump.IsDone && doubleJump.IsReady);
        fsm.AddTransition(jump, airSwordAttack1, () => input.PrimaryAttack.Perform && character.IsEquipped);

        fsm.AddTransition(doubleJump, roll, () => input.Roll.Perform);
        fsm.AddTransition(doubleJump, airSwordAttack1, () => input.PrimaryAttack.Perform && character.IsEquipped);
        
        fsm.AddTransition(fall, idle, () => character.IsGrounded && !input.HasValueX && !character.IsEquipped);
        fsm.AddTransition(fall, run,  () => character.IsGrounded &&  input.HasValueX && !character.IsEquipped);
        fsm.AddTransition(fall, roll, () => input.Roll.Perform);
        fsm.AddTransition(fall, swordIdle,  () => character.IsGrounded && !input.HasValueX && character.IsEquipped);
        fsm.AddTransition(fall, swordRun,   () => character.IsGrounded &&  input.HasValueX && character.IsEquipped);
        fsm.AddTransition(fall, punch3,     () => input.PrimaryAttack.Perform && !character.IsEquipped);
        fsm.AddTransition(fall, airSwordAttack1, () => input.PrimaryAttack.Perform && character.IsEquipped);
        fsm.AddTransition(fall, doubleJump,      () => input.Jump.Start && doubleJump.IsReady);

        fsm.AddTransition(roll, idle, () => roll.IsDone && !input.HasValueX && !character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, run,  () => roll.IsDone &&  input.HasValueX && !character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, jump, () => input.Jump.Start);
        fsm.AddTransition(roll, swordIdle, () => roll.IsDone && !input.HasValueX && character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, swordRun,  () => roll.IsDone &&  input.HasValueX && character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, punch1,       () => input.PrimaryAttack.Perform && !character.IsEquipped);
        fsm.AddTransition(roll, swordAttack1, () => input.PrimaryAttack.Perform &&  character.IsEquipped);

        fsm.AddTransition(swordAttack1, swordAttack2, () => swordAttack1.Elapsed > 0.3f && input.PrimaryAttack.Perform);
        fsm.AddTransition(swordAttack1, swordIdle, () => swordAttack1.IsDone && !input.HasValueX);
        fsm.AddTransition(swordAttack1, swordRun,  () => swordAttack1.IsDone &&  input.HasValueX);
        
        fsm.AddTransition(swordAttack2, swordAttack3,   () => swordAttack2.Elapsed > 0.4f && input.PrimaryAttack.Perform);
        fsm.AddTransition(swordAttack2, swordIdle, () => swordAttack2.IsDone && !input.HasValueX);
        fsm.AddTransition(swordAttack2, swordRun,  () => swordAttack2.IsDone &&  input.HasValueX);
        
        fsm.AddTransition(swordAttack3, swordIdle, () => swordAttack3.IsDone && !input.HasValueX);
        fsm.AddTransition(swordAttack3, swordRun,  () => swordAttack3.IsDone &&  input.HasValueX);
        
        fsm.AddTransition(punch1, punch2, () => punch1.Elapsed > 0.3f && input.PrimaryAttack.Perform);
        fsm.AddTransition(punch1, idle,   () => punch1.IsDone && !input.HasValueX);
        fsm.AddTransition(punch1, run,    () => punch1.IsDone &&  input.HasValueX);
        
        fsm.AddTransition(punch2, idle, () => punch2.IsDone && !input.HasValueX);
        fsm.AddTransition(punch2, run,  () => punch2.IsDone &&  input.HasValueX);
        
        fsm.AddTransition(airSwordAttack1, fall,      () => airSwordAttack1.IsDone && character.IsFalling);
        fsm.AddTransition(airSwordAttack1, swordIdle, () => airSwordAttack1.IsDone && !input.HasValueX && character.IsGrounded);
        fsm.AddTransition(airSwordAttack1, swordRun,  () => airSwordAttack1.IsDone &&  input.HasValueX && character.IsGrounded);
        
        fsm.AddTransition(punch3, fall, () => punch3.IsDone && character.IsFalling);
        fsm.AddTransition(punch3, idle, () => punch3.IsDone && !input.HasValueX && character.IsGrounded);
        fsm.AddTransition(punch3, run,  () => punch3.IsDone &&  input.HasValueX && character.IsGrounded);
        
        // fsm.AddTransition(FSM.AnyState, dead, () => Input.GetKeyDown(KeyCode.D));
        fsm.AddTransition(FSM.AnyState, knockdown, () => input.KnockDown.Start);
        fsm.AddTransition(FSM.AnyState, fall,      () => character.IsFalling && !character.InAction && !character.IsDead);
        
        fsm.AddTransition(knockdown, getup, () => (knockdown.IsDone && character.IsGrounded) && (input.HasValueX || input.Jump.Start));
        
        fsm.AddTransition(getup, idle,      () => getup.IsDone && !character.IsEquipped);
        fsm.AddTransition(getup, swordIdle, () => getup.IsDone &&  character.IsEquipped);
        
        fsm.SetEntry(idle);
    }
    
    private void Update()
    {
        fsm.Update();
        
        if (character.RB2D.position.y < -20)
        {
            character.SprRenderer.enabled = false;
            character.RB2D.position    = new Vector2(-1.5f, 7);
            character.SprRenderer.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }
}