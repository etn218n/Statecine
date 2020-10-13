using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Animation Preset.asset")]
public class AnimationPreset : ScriptableObject
{
    [Header("Locomotion")]
    [SerializeField] private string idle   = string.Empty;
    [SerializeField] private string walk   = string.Empty;
    [SerializeField] private string run    = string.Empty;
    [SerializeField] private string roll   = string.Empty;
    [SerializeField] private string sprint = string.Empty;
    [SerializeField] private string crouch = string.Empty;
    [SerializeField] private string crouchwalk = string.Empty;
    [SerializeField] private string knockdown  = string.Empty;
    [SerializeField] private string getup = string.Empty;
    [SerializeField] private string dead  = string.Empty;
    
    [Header("Equipped Locomotion")]
    [SerializeField] private string swordIdle = string.Empty;
    [SerializeField] private string swordRun  = string.Empty;
    
    [Header("Arial")]
    [SerializeField] private string jump = string.Empty;
    [SerializeField] private string fall = string.Empty;
    
    [Header("Weapon Combat")]
    [SerializeField] private string drawSword    = string.Empty;
    [SerializeField] private string sheathSword  = string.Empty;
    [SerializeField] private string swordAttack1 = string.Empty;
    [SerializeField] private string swordAttack2 = string.Empty;
    [SerializeField] private string swordAttack3 = string.Empty;
    [SerializeField] private string airSwordAttack1 = string.Empty;
    [SerializeField] private string airSwordAttack2 = string.Empty;
    [SerializeField] private string airSwordAttack3 = string.Empty;
    
    [Header("Hand Combat")]
    [SerializeField] private string punch1 = string.Empty;
    [SerializeField] private string punch2 = string.Empty;
    [SerializeField] private string kick1  = string.Empty;
    [SerializeField] private string kick2  = string.Empty;
    
    [SerializeField, HideInInspector] private int idleHashID;
    [SerializeField, HideInInspector] private int walkHashID;
    [SerializeField, HideInInspector] private int runHashID;
    [SerializeField, HideInInspector] private int rollHashID;
    [SerializeField, HideInInspector] private int sprintHashID;
    [SerializeField, HideInInspector] private int crouchHashID;
    [SerializeField, HideInInspector] private int crouchWalkHashID;
    [SerializeField, HideInInspector] private int knockdownHashID;
    [SerializeField, HideInInspector] private int getupHashID;
    [SerializeField, HideInInspector] private int deadHashID;
    
    [SerializeField, HideInInspector] private int swordIdleHashID;
    [SerializeField, HideInInspector] private int swordRunHashID;
    
    [SerializeField, HideInInspector] private int jumpHashID;
    [SerializeField, HideInInspector] private int fallHashID;

    [SerializeField, HideInInspector] private int drawSwordHashID;
    [SerializeField, HideInInspector] private int sheathSwordHashID;
    [SerializeField, HideInInspector] private int swordAttack1HashID;
    [SerializeField, HideInInspector] private int swordAttack2HashID;
    [SerializeField, HideInInspector] private int swordAttack3HashID;
    [SerializeField, HideInInspector] private int airSwordAttack1HashID;
    [SerializeField, HideInInspector] private int airSwordAttack2HashID;
    [SerializeField, HideInInspector] private int airSwordAttack3HashID;
    
    [SerializeField, HideInInspector] private int punch1HashID;
    [SerializeField, HideInInspector] private int punch2HashID;
    [SerializeField, HideInInspector] private int kick1HashID;
    [SerializeField, HideInInspector] private int kick2HashID;

    public int IdleHashID => idleHashID;
    public int WalkHashID => walkHashID;
    public int RunHashID  => runHashID;
    public int RollHashID => rollHashID;
    public int SprintHashID => sprintHashID;
    public int CrouchHashID => crouchHashID;
    public int CrouchWalkHashID => crouchWalkHashID;
    public int KnockdownHashID  => knockdownHashID;
    public int GetUpHashID => getupHashID;
    public int DeadHashID  => deadHashID;

    public int SwordIdleHashID => swordIdleHashID;
    public int SwordRunHashID  => swordRunHashID;

    public int JumpHashID => jumpHashID;
    public int FallHashID => fallHashID;

    public int DrawSwordHashID    => drawSwordHashID;
    public int SheathSwordHashID  => sheathSwordHashID;
    public int SwordAttack1HashID => swordAttack1HashID;
    public int SwordAttack2HashID => swordAttack2HashID;
    public int SwordAttack3HashID => swordAttack3HashID;
    public int AirSwordAttack1HashID => airSwordAttack1HashID;
    public int AirSwordAttack2HashID => airSwordAttack2HashID;
    public int AirSwordAttack3HashID => airSwordAttack3HashID;
    
    public int Punch1HashID => punch1HashID;
    public int Punch2HashID => punch2HashID;
    public int Kick1HashID  => kick1HashID;
    public int Kick2HashID  => kick2HashID;
    
    [Button(Name = "Hash")]
    private void Hash()
    {
        // Locomotion
        idleHashID   = Animator.StringToHash(idle);
        walkHashID   = Animator.StringToHash(walk);
        runHashID    = Animator.StringToHash(run);
        rollHashID   = Animator.StringToHash(roll);
        sprintHashID = Animator.StringToHash(sprint);
        crouchHashID = Animator.StringToHash(crouch);
        crouchWalkHashID = Animator.StringToHash(crouchwalk);
        knockdownHashID  = Animator.StringToHash(knockdown);
        getupHashID = Animator.StringToHash(getup);
        deadHashID  = Animator.StringToHash(dead);
        
        // Equipped Locomotion
        swordIdleHashID = Animator.StringToHash(swordIdle);
        swordRunHashID  = Animator.StringToHash(swordRun);
        
        // Arial
        jumpHashID = Animator.StringToHash(jump);
        fallHashID = Animator.StringToHash(fall);
        
        // Weapon Combat
        drawSwordHashID    = Animator.StringToHash(drawSword);
        sheathSwordHashID  = Animator.StringToHash(sheathSword);
        swordAttack1HashID = Animator.StringToHash(swordAttack1);
        swordAttack2HashID = Animator.StringToHash(swordAttack2);
        swordAttack3HashID = Animator.StringToHash(swordAttack3);
        airSwordAttack1HashID = Animator.StringToHash(airSwordAttack1);
        airSwordAttack2HashID = Animator.StringToHash(airSwordAttack2);
        airSwordAttack3HashID = Animator.StringToHash(airSwordAttack3);
        
        // Hand Combat
        punch1HashID = Animator.StringToHash(punch1);
        punch2HashID = Animator.StringToHash(punch2);
        kick1HashID  = Animator.StringToHash(kick1);
        kick2HashID  = Animator.StringToHash(kick2);
    }
}
