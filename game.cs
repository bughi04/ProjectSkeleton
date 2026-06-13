using System.Diagnostics;
using Silk.NET.SDL;
using TheAdventure.Entities;
using TheAdventure.Exceptions;
using TheAdventure.Managers;
using TheAdventure.Rendering;
namespace TheAdventure;
/// Owns the game loop, all entity lists, and orchestrates input, update, render.
public sealed class Game : IDisposable
{
    private const int   WinW              = 800;
    private const int   WinH              = 800;
    private const int   GroundY           = 750;
    private const float BirdStartX        = 150f;
    private const float BirdStartY        = 360f;
    private const float FirstPipeDelay    = 1.8f;
    private const float PipeInterval      = 2.2f;
    private const int   PipeWidth         = 82;
    private const int   PlanesUnlockScore = 3;

    private readonly Sdl          _sdl;
    private readonly nint         _window;
    private readonly nint         _rendPtr;
    private readonly GameRenderer _renderer;

    private readonly ScoreManager      _scores     = new();
    private readonly DifficultyManager _difficulty = new();

    private GameState _state = GameState.Menu;

    private Bird?           _bird;
    private readonly List<PipePair> _pipes  = [];
    private readonly List<Plane>    _planes = [];

    private int   _score;
    private bool  _isNewRecord;
    private float _pipeTimer;
    private float _planeTimer;

    private bool _spaceWasDown;

    private (float X, float Y, int W)[] _clouds =
    [
        (120f,  75f, 150),
        (370f,  45f, 120),
        (610f,  88f, 175),
        (740f,  38f,  95),
    ];

    private bool _disposed;
    private readonly Random _rng = new();

    public Game(Sdl sdl, nint window, nint rendPtr)
    {
        _sdl      = sdl ?? throw new ArgumentNullException(nameof(sdl));
        _window   = window;
        _rendPtr  = rendPtr;
        _renderer = new GameRenderer(sdl, rendPtr);
    }

    public async Task InitializeAsync()
        => await _scores.LoadAsync().ConfigureAwait(false);

    public void Run()
    {
        ReadOnlySpan<byte> keys;
        unsafe
        {
            keys = new ReadOnlySpan<byte>(_sdl.GetKeyboardState(null), (int)KeyCode.Count);
        }

        var ev     = new Event();
        var timer  = new Stopwatch();
        bool quit  = false;
        timer.Start();

        while (!quit)
        {
            while (_sdl.PollEvent(ref ev) != 0)
            {
                if (ev.Type == (uint)EventType.Quit)
                { quit = true; break; }

                if (ev.Type    == (uint)EventType.Windowevent &&
                    ev.Window.Event == (byte)WindowEventID.TakeFocus)
                {
                    unsafe { _sdl.SetWindowInputFocus(_sdl.GetWindowFromID(ev.Window.WindowID)); }
                }

                if (ev.Type is (uint)EventType.Mousebuttondown or (uint)EventType.Fingerdown)
                    HandleTap();
            }

            if (quit) break;

            double dt = Math.Min(timer.Elapsed.TotalSeconds, 0.05);
            timer.Restart();

            bool spaceDown = keys[(byte)KeyCode.Space] > 0
                          || keys[(byte)KeyCode.Up]    > 0
                          || keys[(byte)KeyCode.W]     > 0;

            bool escJust = keys[(byte)KeyCode.Escape] > 0;
            if (escJust) { quit = true; break; }

            Update(dt, spaceDown);
            _spaceWasDown = spaceDown;

            Render();
        }
    }

    private void Update(double dt, bool spaceDown)
    {
        bool spaceJust = spaceDown && !_spaceWasDown;

        MoveClouds(dt);

        switch (_state)
        {
            case GameState.Menu:
                if (spaceJust) StartGame();
                break;

            case GameState.Playing:
                if (spaceJust) _bird?.Flap();
                UpdatePlaying(dt);
                break;

            case GameState.GameOver:
                if (spaceJust) StartGame();
                break;
        }
    }

    private void HandleTap()
    {
        switch (_state)
        {
            case GameState.Menu or GameState.GameOver:
                StartGame();
                break;
            case GameState.Playing:
                _bird?.Flap();
                break;
        }
    }

    private void StartGame()
    {
        _bird        = new Bird(BirdStartX, BirdStartY);
        _score       = 0;
        _isNewRecord = false;
        _pipeTimer   = FirstPipeDelay;
        _difficulty.Reset();
        _planeTimer  = _difficulty.PlaneSpawnInterval;
        _pipes.Clear();
        _planes.Clear();
        _state = GameState.Playing;
    }
    // AI-generated
    private void UpdatePlaying(double dt)
    {
        if (_bird is null)
            throw new GameException("Bird must not be null while in Playing state.");

        _bird.Update(dt);

        if (_bird.Y <= 0 || _bird.Y + _bird.Height >= GroundY)
        { EndGame(); return; }

        _pipeTimer -= (float)dt;
        if (_pipeTimer <= 0f)
        { SpawnPipe(); _pipeTimer = PipeInterval; }

        if (_score >= PlanesUnlockScore)
        {
            _planeTimer -= (float)dt;
            if (_planeTimer <= 0f)
            { SpawnPlane(); _planeTimer = _difficulty.PlaneSpawnInterval; }
        }

        UpdateEntities(_pipes,  dt);
        UpdateEntities(_planes, dt);

        foreach (var pipe in _pipes)
        {
            if (!pipe.Scored && _bird.X > pipe.X + pipe.Width)
            {
                pipe.Scored = true;
                _score++;
                _difficulty.UpdateScore(_score);
                if (_scores.SetCurrentScore(_score))
                    _isNewRecord = true;
            }

            if (pipe.CollidesWithBird(_bird))
            { EndGame(); return; }
        }

        foreach (var plane in _planes)
        {
            if (plane.CollidesWithBird(_bird))
            { EndGame(); return; }
        }

        _pipes.RemoveAll( static p => !p.IsAlive);
        _planes.RemoveAll(static p => !p.IsAlive);
    }
    // end AI-generated
    private static void UpdateEntities<T>(List<T> list, double dt) where T : IEntity
    {
        foreach (var e in list)
            e.Update(dt);
    }

    private void SpawnPipe()
    {
        int gapCenter = _rng.Next(180, GroundY - 180);
        _pipes.Add(new PipePair(
            WinW + 10f,
            gapCenter,
            _difficulty.GapSize,
            _difficulty.PipeSpeed,
            PipeWidth,
            GroundY));
    }

    private void SpawnPlane()
    {
        float planeY = PickPlaneY();

        bool  fromLeft  = _rng.Next(2) == 0;
        float speed     = _difficulty.PlaneSpeed;
        float startX    = fromLeft ? -Plane.PlaneWidth - 10f : WinW + 10f;
        float signedSpd = fromLeft ? speed : -speed;

        _planes.Add(new Plane(startX, planeY, signedSpd, WinW));
    }

    private float PickPlaneY()
    {
        var target = _pipes
            .Where(p => p.X > BirdStartX)
            .OrderBy(p => p.X)
            .FirstOrDefault();

        if (target is not null)
        {
            float minY = target.GapTop    + 5f;
            float maxY = target.GapBottom - Plane.PlaneHeight - 5f;
            if (maxY > minY)
                return minY + (float)_rng.NextDouble() * (maxY - minY);
            return target.GapCenter - Plane.PlaneHeight / 2f;
        }

        return _rng.Next(160, GroundY - 110);
    }

    private void MoveClouds(double dt)
    {
        for (int i = 0; i < _clouds.Length; i++)
        {
            var (x, y, w) = _clouds[i];
            x -= 28f * (float)dt;
            if (x + w < 0) x = WinW + 5;
            _clouds[i] = (x, y, w);
        }
    }

    private void EndGame()
    {
        _bird?.Kill();
        _state = GameState.GameOver;
        _ = _scores.SaveAsync();
    }
    // AI-generated
    private void Render()
    {
        unsafe
        {
            _sdl.SetRenderDrawColor((Renderer*)_rendPtr, 0, 0, 0, 255);
            _sdl.RenderClear((Renderer*)_rendPtr);
        }

        _renderer.DrawBackground(GroundY);

        foreach (var (x, y, w) in _clouds)
            _renderer.DrawCloud((int)x, (int)y, w);

        foreach (var pipe  in _pipes)  _renderer.DrawPipe(pipe, GroundY);
        foreach (var plane in _planes) _renderer.DrawPlane(plane);

        if (_bird is not null)
            _renderer.DrawBird(_bird);

        switch (_state)
        {
            case GameState.Menu:
                _renderer.DrawMenu(_scores.HighScore);
                break;
            case GameState.Playing:
                _renderer.DrawHudScore(_score, _difficulty.DifficultyLevel);
                break;
            case GameState.GameOver:
                _renderer.DrawGameOver(_score, _scores.HighScore, _isNewRecord);
                break;
        }

        unsafe { _sdl.RenderPresent((Renderer*)_rendPtr); }
    }
    // end AI-generated
    public void Dispose()
    {
        if (!_disposed)
        {
            _scores.SaveAsync().GetAwaiter().GetResult();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
