using System;

// Rectangle class
class Rectangle : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public override void Draw(Canvas canvas)
    {
        for (int i = -Height / 2; i < Height / 2; ++i)
        {
            for (int j = -Width / 2; j < Width / 2; ++j)
            {
                if (X + j >= 0 && X + j < Canvas.Width &&
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
        for (int i = -Height / 2; i < Height / 2; ++i)
        {
            for (int j = -Width / 2; j < Width / 2; ++j)
            {
                if (X + j >= 0 && X + j < Canvas.Width &&
                    Y + i >= 0 && Y + i < Canvas.Height)
                {
                    canvas.CanvasData[Y + i][X + j] = ' ';
                }
            }
        }
    }
}