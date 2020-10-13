using UnityEngine;

public class Character2D : MonoBehaviour
{
    public enum Facing { Left = -1, Right = 1 }
    
    private Animator anim = null;
    private Rigidbody2D rb2d = null;
    private SpriteRenderer sprRenderer = null;

    [SerializeField] private Sensor groundSensor = null;

    public Animator Anim => anim;
    public Rigidbody2D RB2D => rb2d;
    public SpriteRenderer SprRenderer => sprRenderer;

    public float RunSpeed    = 1;
    public float RollSpeed   = 5;
    public float SprintSpeed = 7;
    public float CrouchSpeed = 2;
    public float JumpForce   = 10;

    public Facing FacingDirection { get; private set; }

    public bool IsFalling  => rb2d.velocity.y < 0 && !groundSensor.IsColliding;
    public bool IsLauching => rb2d.velocity.y > 0 && !groundSensor.IsColliding;
    public bool IsGrounded => groundSensor.IsColliding;
    public bool IsOnAir    => !groundSensor.IsColliding && rb2d.velocity.y != 0;
    public bool InAction { get; set; }
    public bool IsDead { get; private set; }
    public bool IsEquipped { get; private set; }

    private int idleHashID   = Animator.StringToHash("Adventurer Idle");
    private int runHashID    = Animator.StringToHash("Adventurer Run");
    private int rollHashID   = Animator.StringToHash("Adventurer Roll");
    private int sprintHashID = Animator.StringToHash("Adventurer Sprint");
    private int jumpHashID   = Animator.StringToHash("Adventurer Jump");
    private int fallHashID   = Animator.StringToHash("Adventurer Fall");
    private int deadHashID   = Animator.StringToHash("Adventurer Dead");
    
    private int crouchHashID     = Animator.StringToHash("Adventurer Crouch");
    private int crouchWalkHashID = Animator.StringToHash("Adventurer Crouch Walk");
    
    private int attack1HashID = Animator.StringToHash("Adventurer Attack1");
    private int attack2HashID = Animator.StringToHash("Adventurer Attack2");
    private int attack3HashID = Animator.StringToHash("Adventurer Attack3");
    private int airAttack1HashID = Animator.StringToHash("Adventurer Air Attack1");

    private int drawSwordHashID   = Animator.StringToHash("Adventurer Draw Sword");
    private int sheathSwordHashID = Animator.StringToHash("Adventurer Sheath Sword");
    
    private int swordIdleHashID = Animator.StringToHash("Adventurer Sword Idle");
    private int swordRunHashID  = Animator.StringToHash("Adventurer Sword Run");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayIdleAnimation()   => anim.Play(idleHashID);
    public void PlayRunAnimation()    => anim.Play(runHashID);
    public void PlayRollAnimation()   => anim.Play(rollHashID);
    public void PlaySprintAnimation() => anim.Play(sprintHashID);
    public void PlayJumpAnimation()   => anim.Play(jumpHashID);
    public void PlayFallAnimation()   => anim.Play(fallHashID);
    public void PlayDeadAnimation()   => anim.Play(deadHashID);
    
    public void PlayCrouchAnimation()     => anim.Play(crouchHashID);
    public void PlayCrouchWalkAnimation() => anim.Play(crouchWalkHashID);
    
    public void PlayDrawSwordAnimation()   => anim.Play(drawSwordHashID);
    public void PlaySheathSwordAnimation() => anim.Play(sheathSwordHashID);
    public void PlaySwordIdleAnimation() => anim.Play(swordIdleHashID);
    public void PlaySwordRunAnimation()  => anim.Play(swordRunHashID);
    
    public void PlayAttack1Animation() => anim.Play(attack1HashID);
    public void PlayAttack2Animation() => anim.Play(attack2HashID);
    public void PlayAttack3Animation() => anim.Play(attack3HashID);
    public void PlayAirAttack1Animation() => anim.Play(airAttack1HashID);

    public void Run(float directionX) => MoveHorizontal(directionX, RunSpeed);
    public void Sprint(float directionX) => MoveHorizontal(directionX, SprintSpeed);
    public void CrouchWalk(float directionX) => MoveHorizontal(directionX, CrouchSpeed);

    public void MoveHorizontal(float directionX, float speed)
    {
        rb2d.velocity = new Vector3(directionX * speed, rb2d.velocity.y);
        
        OnDirectionChanged(directionX);
    }

    public void OnDirectionChanged(float directionX)
    {
        if (directionX > 0)
        {
            FacingDirection = Facing.Right;
            SprRenderer.flipX  = false;
        }
        else if (directionX < 0)
        {
            FacingDirection = Facing.Left;
            SprRenderer.flipX = true;
        }
    }

    public void DrawWeapon()
    {
        IsEquipped = true;
    }

    public void SheathWeapon()
    {
        IsEquipped = false;
    }

    public void Dead()
    {
        IsDead = true;
    }

    public void Roll()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.velocity = new Vector3((int)FacingDirection * RollSpeed, rb2d.velocity.y);
    }

    public void Stop()
    {
        rb2d.velocity = Vector2.zero;
    }

    public void Jump()
    {
        rb2d.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }
}