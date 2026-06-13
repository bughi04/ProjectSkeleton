namespace TheAdventure.Managers;
/// Derives every difficulty parameter from the current score.
public sealed class DifficultyManager
{
    // AI-generated
    private const int   GapBase    = 220;
    private const int   GapMin     = 130;
    private const float SpeedBase  = 200f;
    private const float SpeedMax   = 390f;
    private const float PlaneSpeedBase    = 210f;
    private const float PlaneSpeedMax     = 470f;
    private const float PlaneIntervalBase =   8f;
    private const float PlaneIntervalMin  =   2.2f;
    private int _score;
    public void UpdateScore(int score) => _score = score;
    public void Reset()                => _score = 0;
    public int GapSize => Math.Max(GapMin, GapBase - _score * 3);
    public float PipeSpeed => Math.Min(SpeedMax, SpeedBase + _score * 6f);
    public float PlaneSpeed => Math.Min(PlaneSpeedMax, PlaneSpeedBase + _score * 9f);
    public float PlaneSpawnInterval =>
        Math.Max(PlaneIntervalMin, PlaneIntervalBase - _score * 0.18f);
    public int DifficultyLevel => _score switch
    {
        < 5  => 1,
        < 10 => 2,
        < 20 => 3,
        < 35 => 4,
        _    => 5
    };
    // end AI-generated
}
