# AI Usage Disclosure

## Tools used

| Tool | Version | How it was used |
|---|---|---|
| Claude | Sonnet 4.6 | Chat-based code suggestions and rubber-ducking. Used mainly while iterating on the gameplay design (physics tuning, difficulty curve) and to generate first drafts of a few self-contained helper methods that I then reviewed and integrated. |
| Claude | Opus 4.8 | Chat-based assistance for the more involved, math-heavy or boilerplate-heavy regions (collision math, the pixel-font glyph table, the SDL drawing helpers). Also used for rubber-ducking architecture decisions and reviewing code I had already written. |

I did **not** use any agentic/autonomous coding tool, autocomplete (e.g. Copilot),
or AI asset generation. All graphics are drawn at runtime in code using SDL
primitives — there are no external image, sound, or font assets in this project.

The overall architecture is mine: I decided on the `IEntity` interface, the
`AirborneEntity` base class, the manager classes (`ScoreManager`,
`DifficultyManager`), the renderer wrapper, the game-state machine, and how the
game loop in `Game.cs` ties everything together. I wrote the entry point, the
input handling, the state transitions, the spawning logic, and the overall flow
by hand.

Where I used AI, it was for well-bounded pieces that were tedious or fiddly to
get right: collision routines, the difficulty-scaling formulas, the hand-coded
5×7 pixel font, and the SDL drawing primitives. I read, tested, and adjusted
every one of these blocks.

## Fully AI-generated regions

The following regions are marked inline and are the fully AI-generated code
in the submission:

| File | Region | What it does |
|---|---|---|
| `Entities/AirborneEntity.cs` | whole class body | shared position/alive state + AABB overlap helper |
| `Entities/Bird.cs` | physics fields + `Flap`/`Update` | gravity + flap impulse integration |
| `Entities/Plane.cs` | fields + `Update`/`CollidesWithBird` | horizontal movement, off-screen cull, collision |
| `Entities/PipePair.cs` | `CollidesWithBird` | pipe/cap collision test |
| `Managers/ScoreManager.cs` | `LoadAsync`/`SaveAsync` | async JSON load/save of high score |
| `Managers/DifficultyManager.cs` | the difficulty formulas | score-derived gap/speed/interval/level |
| `Rendering/PixelFont.cs` | glyph table + `DrawGlyph` | 5×7 bitmap font data and glyph rasterizer |
| `Rendering/GameRenderer.cs` | `DrawPipe`/`DrawPlane`/`DrawBird` | entity sprite drawing |
| `Game.cs` | `UpdatePlaying` and `Render` | per-frame update orchestration and draw order |
