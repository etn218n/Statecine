using UnityEngine;

public class Character2D : MonoBehaviour
{
    private Vector2 direction   = Vector2.right;
    private Vector3 cachedScale = new Vector3(1, 1, 1);

    private Animator anim = null;
    private Rigidbody2D rb2d = null;
    private SpriteRenderer sprRenderer = null;

    [Header("Components")]
    [SerializeField] private Sensor groundSensor = null;
    [SerializeField] private Sensor frontSensor  = null;
    [SerializeField] private Collider2D upperBodyCollider = null;
    [SerializeField] private Collider2D lowerBodyCollider = null;
    [SerializeField] private AnimationPreset animationPreset = null;

    public Sensor GroundSensor => groundSensor;
    public Sensor FrontSensor  => frontSensor;

    public Collider2D UpperBodyCollider => upperBodyCollider;
    public Collider2D LowerBodyCollider => lowerBodyCollider;

    public Animator Anim => anim;
    public Rigidbody2D RB2D => rb2d;
    public SpriteRenderer SprRenderer => sprRenderer;

    [Header("Stats")]
    public float RunSpeed = 1;
    public float RollSpeed = 5;
    public float SprintSpeed = 7;
    public float CrouchSpeed = 2;
    public float JumpForce = 10;
    public float DoubleJumpForce = 12;
    public float AirControlSpeed = 4;

    public bool IsFalling  => rb2d.velocity.y < 0 && !groundSensor.IsColliding;
    public bool IsLauching => rb2d.velocity.y > 0 && !groundSensor.IsColliding;
    public bool IsGrounded => groundSensor.IsColliding;
    public bool IsOnAir => !groundSensor.IsColliding && rb2d.velocity.y != 0;
    public bool InAction { get; set; }
    public bool IsDead { get; private set; }
    public bool IsEquipped { get; private set; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayIdleAnimation()   => anim.Play(animationPreset.IdleHashID);
    public void PlayRunAnimation()    => anim.Play(animationPreset.RunHashID);
    public void PlayRollAnimation()   => anim.Play(animationPreset.RollHashID);
    public void PlaySprintAnimation() => anim.Play(animationPreset.SprintHashID);
    public void PlayJumpAnimation()   => anim.Play(animationPreset.JumpHashID);
    public void PlayGetUpAnimation()  => anim.Play(animationPreset.GetUpHashID);
    public void PlayFallAnimation()   => anim.Play(animationPreset.FallHashID);
    public void PlayDeadAnimation()   => anim.Play(animationPreset.DeadHashID);
    
    public void PlayCrouchAnimation()     => anim.Play(animationPreset.CrouchHashID);
    public void PlayCrouchWalkAnimation() => anim.Play(animationPreset.CrouchWalkHashID);
    public void PlayKnockDownAnimation()  => anim.Play(animationPreset.KnockdownHashID);
    
    public void PlayDrawSwordAnimation()   => anim.Play(animationPreset.DrawSwordHashID);
    public void PlaySheathSwordAnimation() => anim.Play(animationPreset.SheathSwordHashID);
    public void PlaySwordIdleAnimation() => anim.Play(animationPreset.SwordIdleHashID);
    public void PlaySwordRunAnimation()  => anim.Play(animationPreset.SwordRunHashID);
    
    public void PlaySwordAttack1Animation() => anim.Play(animationPreset.SwordAttack1HashID);
    public void PlaySwordAttack2Animation() => anim.Play(animationPreset.SwordAttack2HashID);
    public void PlaySwordAttack3Animation() => anim.Play(animationPreset.SwordAttack3HashID);
    public void PlayPunch1Animation()  => anim.Play(animationPreset.Punch1HashID);
    public void PlayPunch2Animation()  => anim.Play(animationPreset.Punch2HashID);
    public void PlayPunch3Animation()  => anim.Play(animationPreset.Punch3HashID);
    public void PlayKick1Animation()   => anim.Play(animationPreset.Kick1HashID);
    public void PlayKick2Animation()   => anim.Play(animationPreset.Kick2HashID);
    public void PlayAirSwordAttack1Animation() => anim.Play(animationPreset.AirSwordAttack1HashID);

    public void Run(float directionX) => MoveHorizontal(directionX, RunSpeed);
    public void Sprint(float directionX) => MoveHorizontal(directionX, SprintSpeed);
    
    public void CrouchWalk(float directionX) => MoveHorizontal(directionX, CrouchSpeed);

    public void AirControl(float directionX)
    {
        if (frontSensor.IsColliding)
            return;
        
        rb2d.velocity = new Vector3(directionX * AirControlSpeed, rb2d.velocity.y);
        
        OnDirectionChanged(directionX);
    }
    
    public void MoveHorizontal(float directionX, float speed)
    {
        rb2d.velocity = new Vector3(directionX * speed, rb2d.velocity.y);
        
        OnDirectionChanged(directionX);
    }

    public void OnDirectionChanged(float directionX)
    {
        if (directionX == 0)
            return;
        
        if (directionX > 0)
            direction.x = 1;
        else 
            direction.x = -1;

        cachedScale.x = direction.x;
        transform.localScale = cachedScale;
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
    
    public void GetKnockDown()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.velocity = new Vector3(-direction.x * 5, 8);
    }

    public void Roll()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.velocity = new Vector3(direction.x * RollSpeed, rb2d.velocity.y);
    }

    public void Stop()
    {
        rb2d.velocity = Vector2.zero;
    }

    public void Jump()
    {
        rb2d.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }
    
    public void DoubleJump()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(Vector2.up * DoubleJumpForce, ForceMode2D.Impulse);
    }
}