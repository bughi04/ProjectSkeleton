namespace TheAdventure.Entities;
/// A top-and-bottom pipe column with a gap, implements IEntity directly (horizontally).
public sealed class PipePair : IEntity
{
    public const int CapHeight = 25;
    public const int CapExtra  = 10;
    public float X       { get; private set; }
    public float Y       => GapCenter;
    public int   Width   { get; }
    public int   Height  => 0;
    public bool  IsAlive { get; private set; } = true;
    public bool  Scored  { get; set; }
    public float GapCenter { get; }
    public int   GapSize   { get; }
    public float GapTop    => GapCenter - GapSize / 2f;
    public float GapBottom => GapCenter + GapSize / 2f;
    private readonly float _speed;
    private readonly int   _groundY;
    public PipePair(float x, float gapCenter, int gapSize,
                    float speed, int width, int groundY)
    {
        X          = x;
        GapCenter  = gapCenter;
        GapSize    = gapSize;
        _speed     = speed;
        Width      = width;
        _groundY   = groundY;
    }
    public void Update(double deltaTime)
    {
        X -= _speed * (float)deltaTime;
        if (X + Width + CapExtra < 0)
            IsAlive = false;
    }
    // AI-generated
    public bool CollidesWithBird(Bird bird)
    {
        float bx = bird.HitX;
        float by = bird.HitY;
        float br = bx + bird.HitW;
        float bb = by + bird.HitH;
        if (br > X && bx < X + Width)
        {
            if (by < GapTop)    return true;
            if (bb > GapBottom) return true;
        }
        float capL = X - CapExtra;
        float capR = X + Width + CapExtra;
        if (br > capL && bx < capR)
        {
            if (bb > GapTop    - CapHeight && by < GapTop)    return true;
            if (by < GapBottom + CapHeight && bb > GapBottom) return true;
        }
        return false;
    }
    // end AI-generated
}
