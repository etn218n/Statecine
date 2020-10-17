using UnityEngine;

public class BotCommandAdventurer : MonoBehaviour, ICommandAdventurer
{
    public ICharacterAction<float> MoveX { get; } = new FloatAction();
    public ICharacterAction Jump { get; } = new BotAction();
    public ICharacterAction Roll { get; } = new BotAction();
    public ICharacterAction Crouch { get; } = new BotAction();
    public ICharacterAction Sprint { get; } = new BotAction();
    public ICharacterAction KnockDown { get; } = new BotAction();
    public ICharacterAction DrawWeapon { get; } = new BotAction();
    public ICharacterAction SheathWeapon { get; } = new BotAction();
    public ICharacterAction PrimaryAttack   { get; } = new BotAction();
    public ICharacterAction SecondaryAttack { get; } = new BotAction();

    private float elapsed = 0;
    
    private void Update()
    {
        MoveX.Value = 1;

        elapsed += Time.deltaTime;

        if (elapsed > 2)
        {
            int n = Random.Range(0, 3);

            if (n == 1)
            {
                Roll.Set(true);
            }
            else if (n == 2)
            {
                Jump.Set(true);
            }
                
        }

        if (elapsed > 2.5f)
        {
            elapsed = 0;
            Roll.Set(false);
            Jump.Set(false);
        }
    }
}