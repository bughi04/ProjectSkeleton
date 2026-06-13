namespace TheAdventure.Entities;
/// The player-controlled bird. Falls under gravity, flaps on input.
public sealed class Bird : AirborneEntity
{
    // AI-generated
    // Physics constants
    private const float Gravity        =  1200f;
    private const float FlapImpulse    =  -480f;
    private const float MaxFallSpeed   =   800f;
    // Collision box is slightly smaller than the visual for fairness.
    private const float HitboxShrink   =     5f;
    public override int Width  => 40;
    public override int Height => 30;
    // Shrunk hitbox used for collision checks
    public float HitX => X + HitboxShrink;
    public float HitY => Y + HitboxShrink;
    public float HitW => Width  - HitboxShrink * 2f;
    public float HitH => Height - HitboxShrink * 2f;
    public float VelocityY { get; private set; }
    public Bird(float x, float y) : base(x, y) { }
    public void Flap()
    {
        if (IsAlive)
            VelocityY = FlapImpulse;
    }
    public override void Update(double deltaTime)
    {
        if (!IsAlive) return;

        float dt = (float)deltaTime;
        VelocityY = Math.Min(VelocityY + Gravity * dt, MaxFallSpeed);
        Y += VelocityY * dt;
    }
    // end AI-generated
}
