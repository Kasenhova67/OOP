using System;

// Circle class
class Circle : Shape
{
    public int Radius { get; set; }

    public Circle(int x, int y, int radius)
    {
        X = x;
        Y = y;
        Radius = radius;
    }

    public override void Draw(Canvas canvas)
    {
        for (int i = -Radius; i <= Radius; ++i)
        {
            for (int j = -Radius; j <= Radius; ++j)
            {
                if (i * i + j * j <= Radius * Radius &&
                    X + j >= 0 && X + j < Canvas.Width &&
                    Y + i >= 0 && Y + i < Canvas.Height)
                {
                    canvas.CanvasData[Y + i][X + j] = Canvas.FillChar;
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
        for (int i = -Radius; i <= Radius; ++i)
        {
            for (int j = -Radius; j <= Radius; ++j)
            {
                if (i * i + j * j <= Radius * Radius &&
                    X + j >= 0 && X + j < Canvas.Width &&
                    Y + i >= 0 && Y + i < Canvas.Height)
                {
                    canvas.CanvasData[Y + i][X + j] = ' ';
                }
            }
        }
    }
}