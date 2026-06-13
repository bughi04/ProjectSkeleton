namespace TheAdventure.Entities;
/// Common contract for every game object that lives on screen and can be updated.
public interface IEntity
{
    float X { get; }
    float Y { get; }
    int   Width  { get; }
    int   Height { get; }
    bool  IsAlive { get; }
    void Update(double deltaTime);
}
