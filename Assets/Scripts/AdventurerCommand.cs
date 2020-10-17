public interface ICommandAdventurer
{
    ICharacterAction<float> MoveX { get; }
    
    ICharacterAction Jump { get; }
    ICharacterAction Roll { get; }
    ICharacterAction Crouch { get; }
    ICharacterAction Sprint { get; }
    ICharacterAction KnockDown  { get; }
    ICharacterAction DrawWeapon { get; }
    ICharacterAction SheathWeapon { get; }
    ICharacterAction PrimaryAttack   { get; }
    ICharacterAction SecondaryAttack { get; }
}