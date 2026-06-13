namespace TheAdventure.Entities;
/// Abstract base for any entity that moves freely in 2D space (bird, plane), provides shared position, alive-state, and a static AABB helper.
public abstract class AirborneEntity : IEntity
{
    // AI-generated
    public float X { get; protected set; }
    public float Y { get; protected set; }
    public abstract int  Width  { get; }
    public abstract int  Height { get; }
    public bool IsAlive { get; protected set; } = true;
    protected AirborneEntity(float x, float y)
    {
        X = x;
        Y = y;
    }
    public abstract void Update(double deltaTime);
    public void Kill() => IsAlive = false;
    /// Axis-aligned bounding-box overlap test
    protected static bool AabbOverlap(
        float ax, float ay, float aw, float ah,
        float bx, float by, float bw, float bh)
        => ax < bx + bw && ax + aw > bx
        && ay < by + bh && ay + ah > by;
    // end AI-generated
}
