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
        var jump = new JumpState(character, input, 0.05f, true);
        var roll = new RollState(character, 0.45f);
        var fall = new FallState(character, input, true);
        var sprint = new SprintState(character, input);
        var getup  = new GetUpState(character, 0.5f);
        var crouch = new CrouchState(character);
        var crouchWalk = new CrouchWalkState(character, input);
        // var dead = new DeadState(character);
        var swordAttack1State = new SwordAttack1State(character, 0.35f);
        var swordAttack2State = new SwordAttack2State(character, 0.45f);
        var swordAttack3State = new SwordAttack3State(character, 0.45f);
        var airSwordAttack1State = new AirSwordAttack1State(character, 0.35f);
        var punch1 = new Punch1State(character, 0.4f);
        var punch2 = new Punch2State(character, 0.4f);
        var drawSword   = new DrawSwordState(character, 0.4f);
        var sheathSword = new SheathSwordState(character, 0.4f);
        var swordIdle   = new SwordIdleState(character);
        var swordRun    = new SwordRunState(character, input);
        var knockdown   = new KnockDownState(character, 0.4f);
        
        fsm.AddTransition(idle, run,  () => input.HasValue);
        fsm.AddTransition(idle, jump, () => Input.GetKey(KeyCode.Space));
        fsm.AddTransition(idle, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(idle, punch1,    () => Input.GetKey(KeyCode.Z));
        fsm.AddTransition(idle, crouch,    () => Input.GetKey(KeyCode.LeftControl));
        fsm.AddTransition(idle, drawSword, () => Input.GetKeyDown(KeyCode.V));
        
        fsm.AddTransition(crouch, idle,       () => Input.GetKeyUp(KeyCode.LeftControl) && !character.IsEquipped);
        fsm.AddTransition(crouch, swordIdle,  () => Input.GetKeyUp(KeyCode.LeftControl) &&  character.IsEquipped);
        fsm.AddTransition(crouch, roll,       () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(crouch, crouchWalk, () => input.HasValue);

        fsm.AddTransition(crouchWalk, idle, () => Input.GetKeyUp(KeyCode.LeftControl) && !input.HasValue && !character.IsEquipped);
        fsm.AddTransition(crouchWalk, run,  () => Input.GetKeyUp(KeyCode.LeftControl) &&  input.HasValue && !character.IsEquipped);
        fsm.AddTransition(crouchWalk, swordIdle, () => Input.GetKeyUp(KeyCode.LeftControl) && !input.HasValue && character.IsEquipped);
        fsm.AddTransition(crouchWalk, swordRun,  () => Input.GetKeyUp(KeyCode.LeftControl) &&  input.HasValue && character.IsEquipped);
        fsm.AddTransition(crouchWalk, roll,      () => Input.GetKey(KeyCode.C));
        
        fsm.AddTransition(drawSword, swordIdle,   () => drawSword.IsDone);
        fsm.AddTransition(drawSword, sheathSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(drawSword, swordRun,    () => drawSword.IsDone && input.HasValue);
        
        fsm.AddTransition(swordIdle, swordRun,    () => input.HasValue);
        fsm.AddTransition(swordIdle, jump,        () => Input.GetKey(KeyCode.Space));
        fsm.AddTransition(swordIdle, sheathSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(swordIdle, crouch,      () => Input.GetKey(KeyCode.LeftControl));
        fsm.AddTransition(swordIdle, swordAttack1State,     () => Input.GetKey(KeyCode.Z));
        fsm.AddTransition(swordIdle, roll,        () => Input.GetKey(KeyCode.C));
        
        fsm.AddTransition(swordRun, swordIdle,   () => !input.HasValue);
        fsm.AddTransition(swordRun, jump,        () => Input.GetKey(KeyCode.Space));
        fsm.AddTransition(swordRun, sprint,      () => Input.GetKey(KeyCode.LeftShift));
        fsm.AddTransition(swordRun, sheathSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(swordRun, crouch,      () => Input.GetKey(KeyCode.LeftControl));
        fsm.AddTransition(swordRun, roll,        () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(swordRun, swordAttack1State,     () => Input.GetKey(KeyCode.Z));

        fsm.AddTransition(sheathSword, idle,      () => sheathSword.IsDone);
        fsm.AddTransition(sheathSword, drawSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(sheathSword, run,       () => sheathSword.IsDone && input.HasValue);
        
        fsm.AddTransition(run, idle,      () => !input.HasValue);
        fsm.AddTransition(run, drawSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(run, jump,      () => Input.GetKey(KeyCode.Space));
        fsm.AddTransition(run, sprint,    () => Input.GetKey(KeyCode.LeftShift));
        fsm.AddTransition(run, crouch,    () => Input.GetKey(KeyCode.LeftControl));
        fsm.AddTransition(run, roll,      () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(run, punch1,    () => Input.GetKey(KeyCode.Z));
        
        fsm.AddTransition(sprint, jump, () => Input.GetKey(KeyCode.Space));
        fsm.AddTransition(sprint, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(sprint, idle, () => (Input.GetKeyUp(KeyCode.LeftShift) || !input.HasValue) && !character.IsEquipped);
        fsm.AddTransition(sprint, swordIdle, () => (Input.GetKeyUp(KeyCode.LeftShift) || !input.HasValue) && character.IsEquipped);
        fsm.AddTransition(sprint, swordAttack1State, () => Input.GetKey(KeyCode.Z) && character.IsEquipped);
        
        fsm.AddTransition(jump, FSM.PreviousState, () => character.IsGrounded && jump.IsDone);
        fsm.AddTransition(jump, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(jump, airSwordAttack1State,   () => Input.GetKey(KeyCode.Z) && character.IsEquipped);

        fsm.AddTransition(fall, idle, () => character.IsGrounded && !input.HasValue && !character.IsEquipped);
        fsm.AddTransition(fall, run,  () => character.IsGrounded &&  input.HasValue && !character.IsEquipped);
        fsm.AddTransition(fall, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(fall, swordIdle,  () => character.IsGrounded && !input.HasValue && character.IsEquipped);
        fsm.AddTransition(fall, swordRun,   () => character.IsGrounded &&  input.HasValue && character.IsEquipped);
        fsm.AddTransition(fall, airSwordAttack1State, () => Input.GetKey(KeyCode.Z) && character.IsEquipped);

        fsm.AddTransition(roll, idle, () => roll.IsDone && !input.HasValue && !character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, run,  () => roll.IsDone &&  input.HasValue && !character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, swordIdle, () => roll.IsDone && !input.HasValue && character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, swordRun,  () => roll.IsDone &&  input.HasValue && character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, jump,    () => Input.GetKey(KeyCode.Space));
        fsm.AddTransition(roll, swordAttack1State, () => Input.GetKey(KeyCode.Z) && character.IsEquipped);
        
        fsm.AddTransition(swordAttack1State, swordAttack2State,   () => swordAttack1State.Elapsed > 0.3f && Input.GetKey(KeyCode.Z));
        fsm.AddTransition(swordAttack1State, swordIdle, () => swordAttack1State.IsDone && !input.HasValue);
        fsm.AddTransition(swordAttack1State, swordRun,  () => swordAttack1State.IsDone &&  input.HasValue);
        
        fsm.AddTransition(swordAttack2State, swordAttack3State,   () => swordAttack2State.Elapsed > 0.4f && Input.GetKey(KeyCode.Z));
        fsm.AddTransition(swordAttack2State, swordIdle, () => swordAttack2State.IsDone && !input.HasValue);
        fsm.AddTransition(swordAttack2State, swordRun,  () => swordAttack2State.IsDone &&  input.HasValue);
        
        fsm.AddTransition(swordAttack3State, swordIdle, () => swordAttack3State.IsDone && !input.HasValue);
        fsm.AddTransition(swordAttack3State, swordRun,  () => swordAttack3State.IsDone &&  input.HasValue);
        
        fsm.AddTransition(punch1, punch2, () => punch1.Elapsed > 0.3f && Input.GetKey(KeyCode.Z));
        fsm.AddTransition(punch1, idle,   () => punch1.IsDone && !input.HasValue);
        fsm.AddTransition(punch1, run,    () => punch1.IsDone &&  input.HasValue);
        
        fsm.AddTransition(punch2, idle, () => punch2.IsDone && !input.HasValue);
        fsm.AddTransition(punch2, run,  () => punch2.IsDone &&  input.HasValue);
        
        fsm.AddTransition(airSwordAttack1State, fall,      () => airSwordAttack1State.IsDone && character.IsFalling);
        fsm.AddTransition(airSwordAttack1State, swordIdle, () => airSwordAttack1State.IsDone && !input.HasValue && character.IsGrounded);
        fsm.AddTransition(airSwordAttack1State, swordRun,  () => airSwordAttack1State.IsDone &&  input.HasValue && character.IsGrounded);
        
        // fsm.AddTransition(FSM.AnyState, dead, () => Input.GetKeyDown(KeyCode.D));
        fsm.AddTransition(FSM.AnyState, knockdown, () => Input.GetKeyDown(KeyCode.K));
        fsm.AddTransition(FSM.AnyState, fall,      () => character.IsFalling && !character.InAction && !character.IsDead);
        
        fsm.AddTransition(knockdown, getup, () => knockdown.IsDone && (input.HasValue || Input.GetKey(KeyCode.Space)));
        
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