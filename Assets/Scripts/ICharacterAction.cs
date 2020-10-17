public interface ICharacterAction
{
    bool Start   { get; }
    bool Perform { get; }
    bool Cancel  { get; }
}

public interface ICharacterAction<T> : ICharacterAction
{
    T Value{ get; set; }
}