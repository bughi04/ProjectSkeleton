namespace TheAdventure.Entities;
/// An enemy plane that flies horizontally through a pipe gap
public sealed class Plane : AirborneEntity
{
    // AI-generated
    public const int PlaneWidth  = 72;
    public const int PlaneHeight = 26;
    public override int Width  => PlaneWidth;
    public override int Height => PlaneHeight;
    public bool MovingLeft { get; }
    private readonly float _speed;
    private readonly int   _windowWidth;
    public Plane(float x, float y, float speed, int windowWidth) : base(x, y)
    {
        _speed       = speed;
        _windowWidth = windowWidth;
        MovingLeft   = speed < 0;
    }
    public override void Update(double deltaTime)
    {
        X += _speed * (float)deltaTime;
        if (X + Width < -60 || X > _windowWidth + 60)
            IsAlive = false;
    }
    public bool CollidesWithBird(Bird bird)
        => AabbOverlap(bird.HitX, bird.HitY, bird.HitW, bird.HitH,
                       X, Y, Width, Height);
    // end AI-generated
}
