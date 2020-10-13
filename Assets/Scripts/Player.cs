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
        var jump = new JumpState(character, input, 0.05f);
        var roll = new RollState(character, 0.45f);
        var fall = new FallState(character, input);
        var sprint = new SprintState(character, input);
        var crouch = new CrouchState(character);
        var crouchWalk = new CrouchWalkState(character, input);
        // var dead = new DeadState(character);
        var attack1 = new Attack1State(character, 0.35f);
        var attack2 = new Attack2State(character, 0.45f);
        var attack3 = new Attack3State(character, 0.45f);
        var airAttack1  = new AirAttack1State(character, 0.35f);
        var drawSword   = new DrawSwordState(character, 0.4f);
        var sheathSword = new SheathSwordState(character, 0.4f);
        var swordIdle   = new SwordIdleState(character);
        var swordRun    = new SwordRunState(character, input);
        
        fsm.AddTransition(idle, run,  () => input.HasValue);
        fsm.AddTransition(idle, jump, () => Input.GetKey(KeyCode.Space));
        fsm.AddTransition(idle, roll, () => Input.GetKey(KeyCode.C));
        //fsm.AddTransition(idle, attack1, () => Input.GetKey(KeyCode.Z));
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
        fsm.AddTransition(swordIdle, attack1,     () => Input.GetKey(KeyCode.Z));
        
        fsm.AddTransition(swordRun, swordIdle,   () => !input.HasValue);
        fsm.AddTransition(swordRun, jump,        () => Input.GetKey(KeyCode.Space));
        fsm.AddTransition(swordRun, sprint,      () => Input.GetKey(KeyCode.LeftShift));
        fsm.AddTransition(swordRun, sheathSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(swordRun, crouch,      () => Input.GetKey(KeyCode.LeftControl));
        fsm.AddTransition(swordRun, attack1,     () => Input.GetKey(KeyCode.Z));
        
        fsm.AddTransition(sheathSword, idle,      () => sheathSword.IsDone);
        fsm.AddTransition(sheathSword, drawSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(sheathSword, run,       () => sheathSword.IsDone && input.HasValue);
        
        fsm.AddTransition(run, idle,      () => !input.HasValue);
        fsm.AddTransition(run, drawSword, () => Input.GetKeyDown(KeyCode.V));
        fsm.AddTransition(run, jump,      () => Input.GetKey(KeyCode.Space));
        fsm.AddTransition(run, sprint,    () => Input.GetKey(KeyCode.LeftShift));
        fsm.AddTransition(run, crouch,    () => Input.GetKey(KeyCode.LeftControl));
        fsm.AddTransition(run, roll,      () => Input.GetKey(KeyCode.C));
        // fsm.AddTransition(run, attack1, () => Input.GetKeyDown(KeyCode.Z));
        
        fsm.AddTransition(sprint, jump, () => Input.GetKey(KeyCode.Space));
        fsm.AddTransition(sprint, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(sprint, idle, () => (Input.GetKeyUp(KeyCode.LeftShift) || !input.HasValue) && !character.IsEquipped);
        fsm.AddTransition(sprint, swordIdle, () => (Input.GetKeyUp(KeyCode.LeftShift) || !input.HasValue) && character.IsEquipped);
        
        fsm.AddTransition(jump, FSM.PreviousState, () => character.IsGrounded && jump.IsDone);
        fsm.AddTransition(jump, roll, () => Input.GetKey(KeyCode.C));
        fsm.AddTransition(jump, airAttack1,   () => Input.GetKey(KeyCode.Z));
        fsm.AddTransition(FSM.AnyState, fall, () => character.IsFalling && !character.InAction && !character.IsDead);
        
        fsm.AddTransition(fall, idle, () => character.IsGrounded && !input.HasValue && !character.IsEquipped);
        fsm.AddTransition(fall, run,  () => character.IsGrounded &&  input.HasValue && !character.IsEquipped);
        fsm.AddTransition(fall, swordIdle,  () => character.IsGrounded && !input.HasValue && character.IsEquipped);
        fsm.AddTransition(fall, swordRun,   () => character.IsGrounded &&  input.HasValue && character.IsEquipped);
        fsm.AddTransition(fall, airAttack1, () => Input.GetKey(KeyCode.Z));

        fsm.AddTransition(roll, idle, () => roll.IsDone && !input.HasValue && !character.IsEquipped);
        fsm.AddTransition(roll, run,  () => roll.IsDone &&  input.HasValue && !character.IsEquipped);
        fsm.AddTransition(roll, swordIdle, () => roll.IsDone && !input.HasValue && character.IsEquipped);
        fsm.AddTransition(roll, swordRun,  () => roll.IsDone &&  input.HasValue && character.IsEquipped);
        fsm.AddTransition(roll, jump, () => Input.GetKey(KeyCode.Space));
        // fsm.AddTransition(roll, attack1, () => Input.GetKey(KeyCode.Z));
        
        fsm.AddTransition(attack1, attack2,   () => attack1.Elapsed > 0.3f && Input.GetKey(KeyCode.Z));
        fsm.AddTransition(attack1, swordIdle, () => attack1.IsDone && !input.HasValue);
        fsm.AddTransition(attack1, swordRun,  () => attack1.IsDone &&  input.HasValue);
        
        fsm.AddTransition(attack2, attack3,   () => attack2.Elapsed > 0.4f && Input.GetKey(KeyCode.Z));
        fsm.AddTransition(attack2, swordIdle, () => attack2.IsDone && !input.HasValue);
        fsm.AddTransition(attack2, swordRun,  () => attack2.IsDone &&  input.HasValue);
        
        fsm.AddTransition(attack3, swordIdle, () => attack3.IsDone && !input.HasValue);
        fsm.AddTransition(attack3, swordRun,  () => attack3.IsDone &&  input.HasValue);
        
        fsm.AddTransition(airAttack1, fall, () => airAttack1.IsDone && character.IsFalling);
        fsm.AddTransition(airAttack1, swordIdle, () => airAttack1.IsDone && !input.HasValue && character.IsGrounded);
        fsm.AddTransition(airAttack1, swordRun,  () => airAttack1.IsDone &&  input.HasValue && character.IsGrounded);
        
        // fsm.AddTransition(FSM.AnyState, dead, () => Input.GetKeyDown(KeyCode.D));
        
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