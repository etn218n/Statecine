public class BotAction : ICharacterAction
{
    private bool value = false;

    public void Set(bool value) => this.value = value;

    public bool Start   { get => value; }
    public bool Perform { get => value; }
    public bool Cancel  { get => !value; }
}