using Silk.NET.SDL;
using TheAdventure.Entities;
namespace TheAdventure.Rendering;
/// Wraps the SDL renderer and exposes all game-specific drawing operations, all unsafe SDL calls are confined to the private helpers here.
public struct Rect
{
    public int X;
    public int Y;
    public int W;
    public int H;
}
public sealed class GameRenderer
{
    private readonly Sdl  _sdl;
    private readonly nint _rendPtr;

    public GameRenderer(Sdl sdl, nint rendPtr)
    {
        _sdl     = sdl;
        _rendPtr = rendPtr;
    }

    public unsafe void SetDrawColor(byte r, byte g, byte b, byte a = 255)
        => _sdl.SetRenderDrawColor((Renderer*)_rendPtr, r, g, b, a);

    public unsafe void FillRect(int x, int y, int w, int h)
    {
        if (w <= 0 || h <= 0) return;

        var rc = new Silk.NET.Maths.Rectangle<int>(
            new Silk.NET.Maths.Vector2D<int>(x, y),
            new Silk.NET.Maths.Vector2D<int>(w, h)
        );

        _sdl.RenderFillRect((Renderer*)_rendPtr, &rc);
    }

    private unsafe void Outline(int x, int y, int w, int h)
    {
        if (w <= 0 || h <= 0) return;

        var rc = new Silk.NET.Maths.Rectangle<int>(
            new Silk.NET.Maths.Vector2D<int>(x, y),
            new Silk.NET.Maths.Vector2D<int>(w, h)
        );

        _sdl.RenderDrawRect((Renderer*)_rendPtr, &rc);
    }

    private unsafe void Line(int x1, int y1, int x2, int y2)
        => _sdl.RenderDrawLine((Renderer*)_rendPtr, x1, y1, x2, y2);

    public void DrawBackground(int groundY)
    {
        SetDrawColor(80, 140, 210);
        FillRect(0, 0, 800, groundY / 2);
        SetDrawColor(110, 175, 235);
        FillRect(0, groundY / 2, 800, groundY - groundY / 2);

        SetDrawColor(55, 165, 55);
        FillRect(0, groundY, 800, 800 - groundY);

        SetDrawColor(40, 135, 40);
        FillRect(0, groundY + 12, 800, 8);
        FillRect(0, groundY + 28, 800, 5);
    }

    public void DrawCloud(int x, int y, int w)
    {
        SetDrawColor(240, 248, 255);
        FillRect(x,            y + 10, w,       18);
        FillRect(x + w / 6,   y,      w * 2/3, 16);
        FillRect(x + w * 2/5, y - 8,  w / 3,   13);
    }
    // AI-generated
    public void DrawPipe(PipePair pipe, int groundY)
    {
        int px  = (int)pipe.X;
        int gt  = (int)pipe.GapTop;
        int gb  = (int)pipe.GapBottom;
        int pw  = pipe.Width;
        int ch  = PipePair.CapHeight;
        int ce  = PipePair.CapExtra;

        SetDrawColor(38, 148, 38);
        FillRect(px, 0,  pw, gt);
        FillRect(px, gb, pw, groundY - gb);

        SetDrawColor(78, 200, 78);
        FillRect(px + 7, 0,  7, gt);
        FillRect(px + 7, gb, 7, groundY - gb);

        SetDrawColor(38, 148, 38);
        FillRect(px - ce, gt - ch, pw + ce * 2, ch);
        FillRect(px - ce, gb,      pw + ce * 2, ch);

        SetDrawColor(78, 200, 78);
        FillRect(px - ce + 7, gt - ch, 7, ch);
        FillRect(px - ce + 7, gb,      7, ch);

        SetDrawColor(0, 90, 0);
        Outline(px - ce, gt - ch, pw + ce * 2, ch);
        Outline(px - ce, gb,      pw + ce * 2, ch);
    }

    public void DrawPlane(Plane plane)
    {
        int px   = (int)plane.X;
        int py   = (int)plane.Y;
        int pw   = Plane.PlaneWidth;
        int ph   = Plane.PlaneHeight;
        bool lft = plane.MovingLeft;

        SetDrawColor(190, 200, 215);
        FillRect(px + pw / 6, py + 3, pw * 2 / 3, ph - 6);

        SetDrawColor(155, 168, 185);
        FillRect(px, py + ph / 4, pw, ph / 2);

        int noseX = lft ? px : px + pw - 14;
        SetDrawColor(130, 143, 162);
        FillRect(noseX, py + ph / 4, 14, ph / 2);

        int cockX = lft ? px + 14 : px + pw - 30;
        SetDrawColor(70, 100, 150);
        FillRect(cockX, py + ph / 4 - 4, 16, ph / 2 + 4);

        int tailX = lft ? px + pw - 14 : px;
        SetDrawColor(155, 168, 185);
        FillRect(tailX, py, 14, ph / 2);

        SetDrawColor(210, 45, 45);
        int stripeX = lft ? px + 22 : px + pw - 32;
        FillRect(stripeX, py + ph / 4, 10, ph / 2);

        SetDrawColor(50, 55, 65);
        int propX = lft ? px - 2 : px + pw + 1;
        Line(propX, py + 2, propX, py + ph - 2);
        Line(propX - 2, py + ph / 2, propX + 2, py + ph / 2);

        SetDrawColor(60, 70, 100);
        Outline(px, py + ph / 4, pw, ph / 2);
    }

    public void DrawBird(Bird bird)
    {
        int bx = (int)bird.X;
        int by = (int)bird.Y;
        int bw = bird.Width;
        int bh = bird.Height;

        bool flapping = bird.VelocityY < -80f;

        SetDrawColor(255, 205, 0);
        FillRect(bx, by, bw, bh);

        SetDrawColor(225, 155, 0);
        FillRect(bx + 5, by + bh / 2, bw - 14, bh / 2);

        SetDrawColor(255, 240, 90);
        int wingY = flapping ? by + 2 : by + bh / 2 - 2;
        FillRect(bx + 4, wingY, bw / 2, bh / 3);

        SetDrawColor(255, 255, 255);
        FillRect(bx + bw - 16, by + 6, 12, 12);

        SetDrawColor(15, 15, 15);
        FillRect(bx + bw - 12, by + 9,  6,  6);

        SetDrawColor(255, 255, 255);
        FillRect(bx + bw - 10, by + 9,  2,  2);

        SetDrawColor(255, 125, 0);
        FillRect(bx + bw - 1, by + bh / 2 - 6, 11, 5);
        FillRect(bx + bw - 1, by + bh / 2 - 1, 9,  4);

        SetDrawColor(185, 125, 0);
        Outline(bx, by, bw, bh);
    }
    // end AI-generated
    public void DrawHudScore(int score, int level)
    {
        string txt = $"SCORE {score}";
        int tw = PixelFont.GetTextWidth(txt);

        PixelFont.DrawText(this, txt, (800 - tw) / 2 + 2, 22, 0, 0, 0);
        PixelFont.DrawText(this, txt, (800 - tw) / 2,     20, 255, 255, 255);

        string lvl = $"LV {level}";
        PixelFont.DrawText(this, lvl, 18, 20, 255, 220, 80);
    }

    public void DrawMenu(int highScore)
    {
        SetDrawColor(0, 0, 55);
        FillRect(80, 140, 640, 210);
        SetDrawColor(80, 140, 255);
        Outline(80, 140, 640, 210);
        Outline(82, 142, 636, 206);

        string title = "SKY ASSAULT";
        int tw = PixelFont.GetTextWidth(title);
        PixelFont.DrawText(this, title, (800 - tw) / 2 + 2, 172, 0, 80, 0);
        PixelFont.DrawText(this, title, (800 - tw) / 2,     170, 255, 215, 0);

        string sub = "DODGE PIPES AND PLANES";
        int sw = PixelFont.GetTextWidth(sub);
        PixelFont.DrawText(this, sub, (800 - sw) / 2, 225, 180, 200, 255);

        string press = "PRESS SPACE TO FLY";
        int pw = PixelFont.GetTextWidth(press);
        PixelFont.DrawText(this, press, (800 - pw) / 2, 430, 255, 255, 100);

        string ctrl = "W OR UP ARROW TO FLAP";
        int cw = PixelFont.GetTextWidth(ctrl);
        PixelFont.DrawText(this, ctrl, (800 - cw) / 2, 475, 160, 160, 160);

        if (highScore > 0)
        {
            string hs = $"BEST {highScore}";
            int hw = PixelFont.GetTextWidth(hs);
            PixelFont.DrawText(this, hs, (800 - hw) / 2, 545, 255, 215, 0);
        }
    }

    public void DrawGameOver(int score, int highScore, bool isNewRecord)
    {
        SetDrawColor(0, 0, 35);
        FillRect(110, 175, 580, 450);
        SetDrawColor(200, 70, 70);
        Outline(110, 175, 580, 450);
        Outline(112, 177, 576, 446);

        string go = "GAME OVER";
        int gow = PixelFont.GetTextWidth(go);
        PixelFont.DrawText(this, go, (800 - gow) / 2 + 2, 222, 100, 0, 0);
        PixelFont.DrawText(this, go, (800 - gow) / 2,     220, 255, 75,  75);

        string sc = $"SCORE  {score}";
        int scw = PixelFont.GetTextWidth(sc);
        PixelFont.DrawText(this, sc, (800 - scw) / 2, 315, 255, 255, 255);

        if (isNewRecord && score > 0)
        {
            string nr = "NEW BEST!";
            int nw = PixelFont.GetTextWidth(nr);
            PixelFont.DrawText(this, nr, (800 - nw) / 2, 370, 255, 215, 0);
        }
        else
        {
            string hs = $"BEST  {highScore}";
            int hw = PixelFont.GetTextWidth(hs);
            PixelFont.DrawText(this, hs, (800 - hw) / 2, 370, 190, 190, 190);
        }

        string rst = "SPACE TO PLAY AGAIN";
        int rw = PixelFont.GetTextWidth(rst);
        PixelFont.DrawText(this, rst, (800 - rw) / 2, 465, 190, 200, 255);

        string esc = "ESC TO QUIT";
        int ew = PixelFont.GetTextWidth(esc);
        PixelFont.DrawText(this, esc, (800 - ew) / 2, 510, 130, 130, 130);
    }
}
