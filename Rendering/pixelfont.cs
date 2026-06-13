namespace TheAdventure.Rendering;
/// Renders ASCII text using a hand-coded 5×7 pixel font via SDL filled rectangles, each glyph is stored as 7 bytes; each byte is a 5-bit row
public static class PixelFont
{
    private const int CharW   = 5;
    private const int CharH   = 7;
    private const int Scale   = 3;
    private const int Spacing = 6;
    // AI-generated
    private static readonly Dictionary<char, byte[]> Glyphs = new()
    {
        ['A'] = [0b01110, 0b10001, 0b10001, 0b11111, 0b10001, 0b10001, 0b10001],
        ['B'] = [0b11110, 0b10001, 0b10001, 0b11110, 0b10001, 0b10001, 0b11110],
        ['C'] = [0b01111, 0b10000, 0b10000, 0b10000, 0b10000, 0b10000, 0b01111],
        ['D'] = [0b11110, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b11110],
        ['E'] = [0b11111, 0b10000, 0b10000, 0b11100, 0b10000, 0b10000, 0b11111],
        ['F'] = [0b11111, 0b10000, 0b10000, 0b11100, 0b10000, 0b10000, 0b10000],
        ['G'] = [0b01111, 0b10000, 0b10000, 0b10111, 0b10001, 0b10001, 0b01111],
        ['H'] = [0b10001, 0b10001, 0b10001, 0b11111, 0b10001, 0b10001, 0b10001],
        ['I'] = [0b01110, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0b01110],
        ['J'] = [0b00111, 0b00010, 0b00010, 0b00010, 0b10010, 0b10010, 0b01100],
        ['K'] = [0b10001, 0b10010, 0b10100, 0b11000, 0b10100, 0b10010, 0b10001],
        ['L'] = [0b10000, 0b10000, 0b10000, 0b10000, 0b10000, 0b10000, 0b11111],
        ['M'] = [0b10001, 0b11011, 0b10101, 0b10001, 0b10001, 0b10001, 0b10001],
        ['N'] = [0b10001, 0b11001, 0b10101, 0b10011, 0b10001, 0b10001, 0b10001],
        ['O'] = [0b01110, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01110],
        ['P'] = [0b11110, 0b10001, 0b10001, 0b11110, 0b10000, 0b10000, 0b10000],
        ['Q'] = [0b01110, 0b10001, 0b10001, 0b10001, 0b10101, 0b10010, 0b01101],
        ['R'] = [0b11110, 0b10001, 0b10001, 0b11110, 0b10100, 0b10010, 0b10001],
        ['S'] = [0b01111, 0b10000, 0b10000, 0b01110, 0b00001, 0b00001, 0b11110],
        ['T'] = [0b11111, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100],
        ['U'] = [0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01110],
        ['V'] = [0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01010, 0b00100],
        ['W'] = [0b10001, 0b10001, 0b10001, 0b10101, 0b10101, 0b11011, 0b01010],
        ['X'] = [0b10001, 0b10001, 0b01010, 0b00100, 0b01010, 0b10001, 0b10001],
        ['Y'] = [0b10001, 0b01010, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100],
        ['Z'] = [0b11111, 0b00001, 0b00010, 0b00100, 0b01000, 0b10000, 0b11111],
        [' '] = [0b00000, 0b00000, 0b00000, 0b00000, 0b00000, 0b00000, 0b00000],
        ['0'] = [0b01110, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01110],
        ['1'] = [0b00100, 0b01100, 0b00100, 0b00100, 0b00100, 0b00100, 0b01110],
        ['2'] = [0b01110, 0b10001, 0b00001, 0b00110, 0b01000, 0b10000, 0b11111],
        ['3'] = [0b01110, 0b10001, 0b00001, 0b00110, 0b00001, 0b10001, 0b01110],
        ['4'] = [0b00010, 0b00110, 0b01010, 0b10010, 0b11111, 0b00010, 0b00010],
        ['5'] = [0b11111, 0b10000, 0b10000, 0b11110, 0b00001, 0b00001, 0b11110],
        ['6'] = [0b01110, 0b10000, 0b10000, 0b11110, 0b10001, 0b10001, 0b01110],
        ['7'] = [0b11111, 0b00001, 0b00010, 0b00100, 0b01000, 0b01000, 0b01000],
        ['8'] = [0b01110, 0b10001, 0b10001, 0b01110, 0b10001, 0b10001, 0b01110],
        ['9'] = [0b01110, 0b10001, 0b10001, 0b01111, 0b00001, 0b00001, 0b01110],
        ['!'] = [0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0b00000, 0b00100],
        ['-'] = [0b00000, 0b00000, 0b00000, 0b11111, 0b00000, 0b00000, 0b00000],
        ['.'] = [0b00000, 0b00000, 0b00000, 0b00000, 0b00000, 0b01100, 0b01100],
    };
    // end AI-generated
    public static int GetTextWidth(string text)
    {
        int n = text.Count(c => Glyphs.ContainsKey(char.ToUpperInvariant(c)));
        return n == 0 ? 0 : n * (CharW * Scale) + (n - 1) * Spacing;
    }
    public static void DrawText(
        GameRenderer gr, string text, int x, int y,
        byte r, byte g, byte b)
    {
        gr.SetDrawColor(r, g, b);
        int curX = x;
        foreach (char c in text)
        {
            char upper = char.ToUpperInvariant(c);
            if (Glyphs.TryGetValue(upper, out var glyph))
            {
                DrawGlyph(gr, glyph, curX, y);
                curX += CharW * Scale + Spacing;
            }
        }
    }
    // AI-generated
    private static void DrawGlyph(GameRenderer gr, byte[] glyph, int x, int y)
    {
        for (int row = 0; row < CharH && row < glyph.Length; row++)
        {
            byte rowBits = glyph[row];
            for (int col = 0; col < CharW; col++)
            {
                if ((rowBits & (1 << (CharW - 1 - col))) != 0)
                    gr.FillRect(x + col * Scale, y + row * Scale, Scale, Scale);
            }
        }
    }
    // end AI-generated
}
