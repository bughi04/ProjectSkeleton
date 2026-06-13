# Sky Assault

Sky Assault is a Flappy Bird-style arcade game built with C# and SDL2 (via Silk.NET). You control a bird that flaps against gravity to weave through scrolling pipe gaps while enemy planes fly across those same gaps to take you down. Difficulty scales with your score: gaps shrink, everything speeds up, and planes unlock after the third pipe. Your high score persists to disk between runs.

---

## Controls

| Input | Action |
|---|---|
| Space / W / Up Arrow | Flap |
| Mouse click | Flap (or start / restart) |
| ESC | Quit |

---

## How difficulty works

- Every pipe you pass makes the game harder
- Gap between pipes shrinks over time
- Pipes and planes move faster as your score climbs
- Planes only appear after you pass 3 pipes
- Level bracket (LV 1–5) shown in the top-left corner

Your high score is saved automatically between runs.

---

## Build & run

Requires the **.NET 10 SDK** (or VS 2022/2026 with the ".NET desktop development" workload). No external assets, all graphics are drawn at runtime with SDL primitives.

**Visual Studio 2022/2026 (recommended):** open `TheAdventure.sln` and press **F5**. VS restores packages, compiles, and launches automatically.

**Terminal:** with the .NET 10 SDK installed, run `dotnet run` from the project folder.

## Folder structure

Your project folder must look exactly like this before running:

```
SkyAssault/
├── TheAdventure.sln
├── TheAdventure.csproj
│
├── Program.cs
├── GameState.cs
├── Game.cs
├── SdlContext.cs
├── KeyCodes.cs
├── MouseButton.cs
│
├── Entities/
│   ├── IEntity.cs
│   ├── AirborneEntity.cs
│   ├── Bird.cs
│   ├── PipePair.cs
│   └── Plane.cs
│
├── Managers/
│   ├── ScoreManager.cs
│   └── DifficultyManager.cs
│
├── Rendering/
│   ├── GameRenderer.cs
│   └── PixelFont.cs
│
└── Exceptions/
    └── GameException.cs
```

---

## Where your high score is saved

| OS | Location |
|---|---|
| Windows | `%AppData%\SkyAssault\scores.json` |
| macOS | `~/.config/SkyAssault/scores.json` |
| Linux | `~/.config/SkyAssault/scores.json` |

You can delete this file to reset your high score.

---

## Requirements checklist

Before hitting Run, confirm:

- [ ] Visual Studio 2022/2026 installed with ".NET desktop development" workload **OR** .NET 10 SDK installed
- [ ] All files are in the correct folders (see structure above)
- [ ] `TheAdventure.csproj` is the updated version (should contain `TreatWarningsAsErrors`)
- [ ] You opened `TheAdventure.sln` (not just a single `.cs` file)

---

## Troubleshooting

**"SDK not found" or project won't load**
- Make sure you installed the **.NET 10 SDK**, not an older version. In Visual Studio installer, check that ".NET 10" is listed under Individual Components.

**Black screen or window doesn't open**
- Make sure your GPU drivers are up to date. The game uses hardware-accelerated SDL2 rendering.

**"File not found" build errors**
- Double-check that every file is in the right subfolder (`Entities/`, `Managers/`, etc.) and that no filenames have typos.

**High score not saving**
→ This is non-fatal — the game still runs. Check that your user account has write access to `%AppData%`.
