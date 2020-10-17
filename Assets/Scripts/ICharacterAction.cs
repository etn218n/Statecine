public interface ICharacterAction
{
    void Set(bool value);
    bool Start   { get; }
    bool Perform { get; }
    bool Cancel  { get; }
}

public interface ICharacterAction<T> : ICharacterAction
{
    T Value{ get; set; }
}