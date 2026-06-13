# Sky Assault 🐦✈️

A Flappy Bird-style game where enemy planes fly through the pipe gaps trying to take you down. Dodge the columns, avoid the planes, survive as long as you can.

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

## No-terminal option — Visual Studio 2022 (Recommended)

This is the easiest way to run the game with zero terminal usage.

1. Download **Visual Studio 2022 Community** (free):
   👉 https://visualstudio.microsoft.com/vs/community/

2. During installation, tick **".NET desktop development"** workload

3. Open Visual Studio → `File → Open → Project/Solution`

4. Navigate to your `SkyAssault` folder and open `TheAdventure.sln`

5. Press **F5** (or the green ▶ Run button at the top)

That's it — Visual Studio handles restoring packages, compiling, and launching the game automatically.

> **VS Code** also works if you prefer a lighter editor.
> Install it from https://code.visualstudio.com/ then add the
> **"C# Dev Kit"** extension. Open the SkyAssault folder, then
> press F5.

---

## Terminal option (if you prefer)

If you do want to use a terminal:

1. Install the **.NET 10 SDK**:
   👉 https://dotnet.microsoft.com/download/dotnet/10.0
   *(Pick the SDK installer — not just the Runtime)*

2. Open Terminal / Command Prompt / PowerShell inside your `SkyAssault` folder

3. Run:
   ```
   dotnet run
   ```

---

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

### Which files come from where

| File | Source |
|---|---|
| `TheAdventure.sln` | Original skeleton (unchanged) |
| `SdlContext.cs` | Original skeleton (unchanged) |
| `KeyCodes.cs` | Original skeleton (unchanged) |
| `MouseButton.cs` | Original skeleton (unchanged) |
| Everything else | Written for this project (copy from artifacts) |

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

- [ ] Visual Studio 2022 installed with ".NET desktop development" workload **OR** .NET 10 SDK installed
- [ ] All files are in the correct folders (see structure above)
- [ ] `TheAdventure.csproj` is the updated version (should contain `TreatWarningsAsErrors`)
- [ ] You opened `TheAdventure.sln` (not just a single `.cs` file)

---

## Troubleshooting

**"SDK not found" or project won't load**
→ Make sure you installed the **.NET 10 SDK**, not an older version. In Visual Studio installer, check that ".NET 10" is listed under Individual Components.

**Black screen or window doesn't open**
→ Make sure your GPU drivers are up to date. The game uses hardware-accelerated SDL2 rendering.

**"File not found" build errors**
→ Double-check that every file is in the right subfolder (`Entities/`, `Managers/`, etc.) and that no filenames have typos.

**High score not saving**
→ This is non-fatal — the game still runs. Check that your user account has write access to `%AppData%`.
