namespace HyakuServer.Utility
{
    public class Texture2D
    {
        public Color[] Pixels;
        public int Width, Height;

        public Texture2D(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new Color[Width * Height];
        }

        public void SetPixel(int x, int y, Color color)
        {
            Pixels[y * Width + x] = color;
        }
    }

    public class Color
    {
        public float R, G, B, A;

        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}