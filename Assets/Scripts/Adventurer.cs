using NodeCanvas;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    private Character2D character = null;
    private ICommandAdventurer command = null;

    private FSM fsm = new FSM();

    private void Awake()
    {
        character = GetComponent<Character2D>();
        command   = GetComponent<ICommandAdventurer>();
    }

    private void Start()
    {
        var idle = new IdleState(character);
        var run  = new RunState(character, command);
        var jump = new JumpState(character, command, 0.01f, true);
        var doubleJump = new DoubleJumpState(character, command, 0.05f, true);
        var roll = new RollState(character, 0.45f);
        var fall = new FallState(character, command, true);
        var sprint = new SprintState(character, command);
        var getup  = new GetUpState(character, 0.5f);
        var crouch = new CrouchState(character);
        var crouchWalk = new CrouchWalkState(character, command);
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
        var swordRun    = new SwordRunState(character, command);
        var knockdown   = new KnockDownState(character, 0.4f);
        
        fsm.AddTransition(idle, run,  () => command.MoveX.Perform);
        fsm.AddTransition(idle, jump, () => command.Jump.Start);
        fsm.AddTransition(idle, roll, () => command.Roll.Perform);
        fsm.AddTransition(idle, punch1,    () => command.PrimaryAttack.Perform);
        fsm.AddTransition(idle, crouch,    () => command.Crouch.Start);
        fsm.AddTransition(idle, drawSword, () => command.DrawWeapon.Start);
        
        fsm.AddTransition(crouch, idle,       () => command.Crouch.Cancel && !character.IsEquipped);
        fsm.AddTransition(crouch, swordIdle,  () => command.Crouch.Cancel &&  character.IsEquipped);
        fsm.AddTransition(crouch, roll,       () => command.Roll.Perform);
        fsm.AddTransition(crouch, crouchWalk, () => command.MoveX.Perform);

        fsm.AddTransition(crouchWalk, crouch,    () => !command.MoveX.Perform);
        fsm.AddTransition(crouchWalk, idle,      () => command.Crouch.Cancel && !character.IsEquipped);
        fsm.AddTransition(crouchWalk, swordIdle, () => command.Crouch.Cancel &&  character.IsEquipped);
        fsm.AddTransition(crouchWalk, roll,      () => command.Roll.Perform);
        
        fsm.AddTransition(drawSword, swordIdle,   () => drawSword.IsDone);
        fsm.AddTransition(drawSword, sheathSword, () => command.SheathWeapon.Start);
        fsm.AddTransition(drawSword, swordRun,    () => drawSword.IsDone && command.MoveX.Perform);
        
        fsm.AddTransition(swordIdle, swordRun,     () => command.MoveX.Perform);
        fsm.AddTransition(swordIdle, jump,         () => command.Jump.Start);
        fsm.AddTransition(swordIdle, sheathSword,  () => command.SheathWeapon.Start);
        fsm.AddTransition(swordIdle, crouch,       () => command.Crouch.Start);
        fsm.AddTransition(swordIdle, swordAttack1, () => command.PrimaryAttack.Perform);
        fsm.AddTransition(swordIdle, roll,         () => command.Roll.Perform);
        
        fsm.AddTransition(swordRun, swordIdle,    () => command.MoveX.Cancel);
        fsm.AddTransition(swordRun, jump,         () => command.Jump.Start);
        fsm.AddTransition(swordRun, sprint,       () => command.Sprint.Perform);
        fsm.AddTransition(swordRun, sheathSword,  () => command.SheathWeapon.Start);
        fsm.AddTransition(swordRun, crouch,       () => command.Crouch.Start);
        fsm.AddTransition(swordRun, roll,         () => command.Roll.Perform);
        fsm.AddTransition(swordRun, swordAttack1, () => command.PrimaryAttack.Perform);

        fsm.AddTransition(sheathSword, idle,      () => sheathSword.IsDone);
        fsm.AddTransition(sheathSword, drawSword, () => command.DrawWeapon.Start);
        fsm.AddTransition(sheathSword, run,       () => sheathSword.IsDone && command.MoveX.Perform);
        
        fsm.AddTransition(run, idle,      () => command.MoveX.Cancel);
        fsm.AddTransition(run, drawSword, () => command.DrawWeapon.Start);
        fsm.AddTransition(run, jump,      () => command.Jump.Start);
        fsm.AddTransition(run, sprint,    () => command.Sprint.Perform);
        fsm.AddTransition(run, crouch,    () => command.Crouch.Start);
        fsm.AddTransition(run, roll,      () => command.Roll.Perform);
        fsm.AddTransition(run, punch1,    () => command.PrimaryAttack.Perform);
        
        fsm.AddTransition(sprint, jump, () => command.Jump.Start);
        fsm.AddTransition(sprint, roll, () => command.Roll.Perform);
        fsm.AddTransition(sprint, idle, () => (command.Sprint.Cancel || command.MoveX.Cancel) && !character.IsEquipped);
        fsm.AddTransition(sprint, swordIdle,    () => (command.Sprint.Cancel || command.MoveX.Cancel) && character.IsEquipped);
        fsm.AddTransition(sprint, swordAttack1, () => command.PrimaryAttack.Perform && character.IsEquipped);
        
        //fsm.AddTransition(jump, FSM.PreviousState, () => character.IsGrounded && jump.IsDone);
        fsm.AddTransition(jump, roll, () => command.Roll.Perform);
        fsm.AddTransition(jump, punch3, () => command.PrimaryAttack.Perform && !character.IsEquipped);
        fsm.AddTransition(jump, doubleJump, () => command.Jump.Start && jump.IsDone && doubleJump.IsReady);
        fsm.AddTransition(jump, airSwordAttack1, () => command.PrimaryAttack.Perform && character.IsEquipped);

        fsm.AddTransition(doubleJump, roll, () => command.Roll.Perform);
        fsm.AddTransition(doubleJump, airSwordAttack1, () => command.PrimaryAttack.Perform && character.IsEquipped);
        
        fsm.AddTransition(fall, idle, () => character.IsGrounded && command.MoveX.Cancel && !character.IsEquipped);
        fsm.AddTransition(fall, run,  () => character.IsGrounded && command.MoveX.Perform && !character.IsEquipped);
        fsm.AddTransition(fall, roll, () => command.Roll.Perform);
        fsm.AddTransition(fall, swordIdle,  () => character.IsGrounded && command.MoveX.Cancel && character.IsEquipped);
        fsm.AddTransition(fall, swordRun,   () => character.IsGrounded && command.MoveX.Perform && character.IsEquipped);
        fsm.AddTransition(fall, punch3,     () => command.PrimaryAttack.Perform && !character.IsEquipped);
        fsm.AddTransition(fall, airSwordAttack1, () => command.PrimaryAttack.Perform && character.IsEquipped);
        fsm.AddTransition(fall, doubleJump,      () => command.Jump.Start && doubleJump.IsReady);

        fsm.AddTransition(roll, idle, () => roll.IsDone && command.MoveX.Cancel && !character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, run,  () => roll.IsDone && command.MoveX.Perform && !character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, jump, () => command.Jump.Start);
        fsm.AddTransition(roll, swordIdle, () => roll.IsDone && command.MoveX.Cancel && character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, swordRun,  () => roll.IsDone && command.MoveX.Perform && character.IsEquipped && character.IsGrounded);
        fsm.AddTransition(roll, punch1,       () => command.PrimaryAttack.Perform && !character.IsEquipped);
        fsm.AddTransition(roll, swordAttack1, () => command.PrimaryAttack.Perform &&  character.IsEquipped);

        fsm.AddTransition(swordAttack1, swordAttack2, () => swordAttack1.Elapsed > 0.3f && command.PrimaryAttack.Perform);
        fsm.AddTransition(swordAttack1, swordIdle, () => swordAttack1.IsDone && command.MoveX.Cancel);
        fsm.AddTransition(swordAttack1, swordRun,  () => swordAttack1.IsDone && command.MoveX.Perform);
        
        fsm.AddTransition(swordAttack2, swordAttack3,   () => swordAttack2.Elapsed > 0.4f && command.PrimaryAttack.Perform);
        fsm.AddTransition(swordAttack2, swordIdle, () => swordAttack2.IsDone && command.MoveX.Cancel);
        fsm.AddTransition(swordAttack2, swordRun,  () => swordAttack2.IsDone && command.MoveX.Perform);
        
        fsm.AddTransition(swordAttack3, swordIdle, () => swordAttack3.IsDone && command.MoveX.Cancel);
        fsm.AddTransition(swordAttack3, swordRun,  () => swordAttack3.IsDone && command.MoveX.Perform);
        
        fsm.AddTransition(punch1, punch2, () => punch1.Elapsed > 0.3f && command.PrimaryAttack.Perform);
        fsm.AddTransition(punch1, idle,   () => punch1.IsDone && command.MoveX.Cancel);
        fsm.AddTransition(punch1, run,    () => punch1.IsDone && command.MoveX.Perform);
        
        fsm.AddTransition(punch2, idle, () => punch2.IsDone && command.MoveX.Cancel);
        fsm.AddTransition(punch2, run,  () => punch2.IsDone && command.MoveX.Perform);
        
        fsm.AddTransition(airSwordAttack1, fall,      () => airSwordAttack1.IsDone && character.IsFalling);
        fsm.AddTransition(airSwordAttack1, swordIdle, () => airSwordAttack1.IsDone && command.MoveX.Cancel && character.IsGrounded);
        fsm.AddTransition(airSwordAttack1, swordRun,  () => airSwordAttack1.IsDone && command.MoveX.Perform && character.IsGrounded);
        
        fsm.AddTransition(punch3, fall, () => punch3.IsDone && character.IsFalling);
        fsm.AddTransition(punch3, idle, () => punch3.IsDone && command.MoveX.Cancel && character.IsGrounded);
        fsm.AddTransition(punch3, run,  () => punch3.IsDone && command.MoveX.Perform && character.IsGrounded);
        
        // fsm.AddTransition(FSM.AnyState, dead, () => Input.GetKeyDown(KeyCode.D));
        fsm.AddTransition(fsm.AnyNode, knockdown, () => command.KnockDown.Start);
        fsm.AddTransition(fsm.AnyNode, fall,      () => character.IsFalling && !character.InAction && !character.IsDead);
        
        fsm.AddTransition(knockdown, getup, () => (knockdown.IsDone && character.IsGrounded) && (command.MoveX.Perform || command.Jump.Start));
        
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
            character.RB2D.position = new Vector2(-1.5f, 7);
            character.SprRenderer.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }
}