namespace TheAdventure.Exceptions;
/// Thrown when the game reaches an invalid internal state
public sealed class GameException : Exception
{
    public GameException(string message) : base(message) { }
    public GameException(string message, Exception inner) : base(message, inner) { }
}