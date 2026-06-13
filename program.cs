using Silk.NET.SDL;
using TheAdventure;

namespace TheAdventure;

public static class Program
{
    public static void Main()
    {
        var sdl = new Sdl(new SdlContext());

        int initResult = sdl.Init(Sdl.InitVideo | Sdl.InitEvents | Sdl.InitTimer);
        if (initResult < 0)
            throw new InvalidOperationException("Failed to initialize SDL.");

        nint window;
        unsafe
        {
            window = (nint)sdl.CreateWindow(
                "Sky Assault",
                Sdl.WindowposUndefined,
                Sdl.WindowposUndefined,
                800, 800,
                (uint)(WindowFlags.Resizable | WindowFlags.AllowHighdpi)
            );
        }

        if (window == nint.Zero)
        {
            var ex = sdl.GetErrorAsException();
            throw ex ?? new Exception("Failed to create window.");
        }

        nint renderer;
        unsafe
        {
            renderer = (nint)sdl.CreateRenderer(
                (Window*)window, -1, (uint)RendererFlags.Accelerated);

            if (renderer != nint.Zero)
                sdl.RenderSetVSync((Renderer*)renderer, 1);
        }

        if (renderer == nint.Zero)
        {
            var ex = sdl.GetErrorAsException();
            throw ex ?? new Exception("Failed to create renderer.");
        }

        using var game = new Game(sdl, window, renderer);
        game.InitializeAsync().GetAwaiter().GetResult();
        game.Run();

        unsafe
        {
            sdl.DestroyRenderer((Renderer*)renderer);
            sdl.DestroyWindow((Window*)window);
        }

        sdl.Quit();
    }
}
