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
        fsm.AddTransition(idle, jump, () => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow));
        fsm.AddTransition(idle, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(idle, punch1,    () => Input.GetKey(KeyCode.Z));
        fsm.AddTransition(idle, crouch,    () => Input.GetKey(KeyCode.LeftControl));
        fsm.AddTransition(idle, drawSword, () => Input.GetKeyDown(KeyCode.V));
        
        fsm.AddTransition(crouch, idle,       () => Input.GetKeyUp(KeyCode.LeftControl) && !character.IsEquipped);
        fsm.AddTransition(crouch, swordIdle,  () => Input.GetKeyUp(KeyCode.LeftControl) &&  character.IsEquipped);
        fsm.AddTransition(crouch, roll,       () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(crouch, crouchWalk, () => input.HasValueX);

        fsm.AddTransition(crouchWalk, idle, () => Input.GetKeyUp(KeyCode.LeftControl) && !input.HasValueX && !character.IsEquipped);
        fsm.AddTransition(crouchWalk, run,  () => Input.GetKeyUp(KeyCode.LeftControl) &&  input.HasValueX && !character.IsEquipped);
        fsm.AddTransition(crouchWalk, swordIdle, () => Input.GetKeyUp(KeyCode.LeftControl) && !input.HasValueX && character.IsEquipped);
        fsm.AddTransition(crouchWalk, swordRun,  () => Input.GetKeyUp(KeyCode.LeftControl) &&  input.HasValueX && character.IsEquipped);
        fsm.AddTransition(crouchWalk, roll,      () => Input.GetKey(KeyCode.C));
        
        fsm.AddTransition(drawSword, swordIdle,   () => drawSword.IsDone);
        fsm.AddTransition(drawSword, sheathSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(drawSword, swordRun,    () => drawSword.IsDone && input.HasValueX);
        
        fsm.AddTransition(swordIdle, swordRun,    () => input.HasValueX);
        fsm.AddTransition(swordIdle, jump,        () => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow));
        fsm.AddTransition(swordIdle, sheathSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(swordIdle, crouch,      () => Input.GetKey(KeyCode.LeftControl));
        fsm.AddTransition(swordIdle, swordAttack1,     () => Input.GetKey(KeyCode.Z));
        fsm.AddTransition(swordIdle, roll,        () => Input.GetKey(KeyCode.C));
        
        fsm.AddTransition(swordRun, swordIdle,   () => !input.HasValueX);
        fsm.AddTransition(swordRun, jump,        () => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow));
        fsm.AddTransition(swordRun, sprint,      () => Input.GetKey(KeyCode.LeftShift));
        fsm.AddTransition(swordRun, sheathSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(swordRun, crouch,      () => Input.GetKey(KeyCode.LeftControl));
        fsm.AddTransition(swordRun, roll,        () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(swordRun, swordAttack1,     () => Input.GetKey(KeyCode.Z));

        fsm.AddTransition(sheathSword, idle,      () => sheathSword.IsDone);
        fsm.AddTransition(sheathSword, drawSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(sheathSword, run,       () => sheathSword.IsDone && input.HasValueX);
        
        fsm.AddTransition(run, idle,      () => !input.HasValueX);
        fsm.AddTransition(run, drawSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(run, jump,      () => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow));
        fsm.AddTransition(run, sprint,    () => Input.GetKey(KeyCode.LeftShift));
        fsm.AddTransition(run, crouch,    () => Input.GetKey(KeyCode.LeftControl));
        fsm.AddTransition(run, roll,      () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(run, punch1,    () => Input.GetKey(KeyCode.Z));
        
        fsm.AddTransition(sprint, jump, () => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow));
        fsm.AddTransition(sprint, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(sprint, idle, () => (Input.GetKeyUp(KeyCode.LeftShift) || !input.HasValueX) && !character.IsEquipped);
        fsm.AddTransition(sprint, swordIdle, () => (Input.GetKeyUp(KeyCode.LeftShift) || !input.HasValueX) && character.IsEquipped);
        fsm.AddTransition(sprint, swordAttack1, () => Input.GetKey(KeyCode.Z) && character.IsEquipped);
        
        //fsm.AddTransition(jump, FSM.PreviousState, () => character.IsGrounded && jump.IsDone);
        fsm.AddTransition(jump, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(jump, airSwordAttack1, () => Input.GetKey(KeyCode.Z) && character.IsEquipped);
        fsm.AddTransition(jump, punch3, () => Input.GetKey(KeyCode.Z) && !character.IsEquipped);
        fsm.AddTransition(jump, doubleJump, () => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) && jump.IsDone && doubleJump.IsReady);

        fsm.AddTransition(doubleJump, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(doubleJump, airSwordAttack1, () => Input.GetKey(KeyCode.Z) && character.IsEquipped);
        
        fsm.AddTransition(fall, idle, () => character.IsGrounded && !input.HasValueX && !character.IsEquipped);
        fsm.AddTransition(fall, run,  () => character.IsGrounded &&  input.HasValueX && !character.IsEquipped);
        fsm.AddTransition(fall, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(fall, swordIdle,  () => character.IsGrounded && !input.HasValueX && character.IsEquipped);
        fsm.AddTransition(fall, swordRun,   () => character.IsGrounded &&  input.HasValueX && character.IsEquipped);
        fsm.AddTransition(fall, airSwordAttack1, () => Input.GetKey(KeyCode.Z) && character.IsEquipped);
        fsm.AddTransition(fall, punch3, () => Input.GetKey(KeyCode.Z) && !character.IsEquipped);
        fsm.AddTransition(fall, doubleJump, () => Input.GetKeyDown(KeyCode.Space) && doubleJump.IsReady);

        fsm.AddTransition(roll, idle, () => roll.IsDone && !input.HasValueX && !character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, run,  () => roll.IsDone &&  input.HasValueX && !character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, swordIdle, () => roll.IsDone && !input.HasValueX && character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, swordRun,  () => roll.IsDone &&  input.HasValueX && character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, jump,      () => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow));
        fsm.AddTransition(roll, swordAttack1, () => Input.GetKey(KeyCode.Z) &&  character.IsEquipped);
        fsm.AddTransition(roll, punch1, () => Input.GetKey(KeyCode.Z) && !character.IsEquipped);
        
        fsm.AddTransition(swordAttack1, swordAttack2,   () => swordAttack1.Elapsed > 0.3f && Input.GetKey(KeyCode.Z));
        fsm.AddTransition(swordAttack1, swordIdle, () => swordAttack1.IsDone && !input.HasValueX);
        fsm.AddTransition(swordAttack1, swordRun,  () => swordAttack1.IsDone &&  input.HasValueX);
        
        fsm.AddTransition(swordAttack2, swordAttack3,   () => swordAttack2.Elapsed > 0.4f && Input.GetKey(KeyCode.Z));
        fsm.AddTransition(swordAttack2, swordIdle, () => swordAttack2.IsDone && !input.HasValueX);
        fsm.AddTransition(swordAttack2, swordRun,  () => swordAttack2.IsDone &&  input.HasValueX);
        
        fsm.AddTransition(swordAttack3, swordIdle, () => swordAttack3.IsDone && !input.HasValueX);
        fsm.AddTransition(swordAttack3, swordRun,  () => swordAttack3.IsDone &&  input.HasValueX);
        
        fsm.AddTransition(punch1, punch2, () => punch1.Elapsed > 0.3f && Input.GetKey(KeyCode.Z));
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
        fsm.AddTransition(FSM.AnyState, knockdown, () => Input.GetKeyDown(KeyCode.K));
        fsm.AddTransition(FSM.AnyState, fall,      () => character.IsFalling && !character.InAction && !character.IsDead);
        
        fsm.AddTransition(knockdown, getup, () => (knockdown.IsDone && character.IsGrounded) && (input.HasValueX || Input.GetKey(KeyCode.Space)));
        
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