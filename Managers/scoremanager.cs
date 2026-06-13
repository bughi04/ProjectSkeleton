using System.Text.Json;
namespace TheAdventure.Managers;
/// Loads and persists the all-time high score to disk using async I/O, stored as JSON under %AppData%/SkyAssault/
public sealed class ScoreManager
{
    private static readonly string SaveDir  =
        Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "SkyAssault");
    private static readonly string SavePath = Path.Combine(SaveDir, "scores.json");
    private static readonly JsonSerializerOptions JsonOpts =
        new() { WriteIndented = true };
    private ScoreData _data = new();
    public int HighScore    => _data.HighScore;
    public int CurrentScore { get; private set; }
    // AI-generated
    public async Task LoadAsync()
    {
        try
        {
            if (!File.Exists(SavePath)) return;

            string json = await File.ReadAllTextAsync(SavePath)
                                    .ConfigureAwait(false);
            _data = JsonSerializer.Deserialize<ScoreData>(json) ?? new ScoreData();
        }
        catch (Exception)
        {
            _data = new ScoreData();
        }
    }
    public async Task SaveAsync()
    {
        try
        {
            Directory.CreateDirectory(SaveDir);
            string json = JsonSerializer.Serialize(_data, JsonOpts);
            await File.WriteAllTextAsync(SavePath, json).ConfigureAwait(false);
        }
        catch (Exception)
        {

        }
    }
    // end AI-generated
    public bool SetCurrentScore(int score)
    {
        CurrentScore = score;
        if (score <= _data.HighScore) return false;

        _data.HighScore    = score;
        _data.LastAchieved = DateTime.UtcNow;
        return true;
    }
    private sealed class ScoreData
    {
        public int      HighScore    { get; set; }
        public DateTime LastAchieved { get; set; } = DateTime.UtcNow;
    }
}
