using System;

// Triangle class (Simplified Isosceles)
class Triangle : Shape
{
    public int BaseWidth { get; set; }
    public int Height { get; set; }

    public Triangle(int x, int y, int baseWidth, int height)
    {
        X = x;
        Y = y;
        BaseWidth = baseWidth;
        Height = height;
    }

    public override void Draw(Canvas canvas)
    {
        for (int i = 0; i < Height; ++i)
        {
            for (int j = -BaseWidth / 2; j < BaseWidth / 2; ++j)
            {
                if (Math.Abs(j) <= (double)BaseWidth / (2 * Height) * (Height - i) &&
                    X + j >= 0 && X + j < Canvas.Width &&
                    Y - i >= 0 && Y - i < Canvas.Height)
                {
                    canvas.CanvasData[Y - i][X + j] = Canvas.FillChar;
                }
            }
        }
    }

    public override void Fill(Canvas canvas)
    {
        Draw(canvas);
    }

    public override void Erase(Canvas canvas)
    {
        for (int i = 0; i < Height; ++i)
        {
            for (int j = -BaseWidth / 2; j < BaseWidth / 2; ++j)
            {
                if (Math.Abs(j) <= (double)BaseWidth / (2 * Height) * (Height - i) &&
                    X + j >= 0 && X + j < Canvas.Width &&
                    Y - i >= 0 && Y - i < Canvas.Height)
                {
                    canvas.CanvasData[Y - i][X + j] = ' ';
                }
            }
        }
    }
}
