using System;
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
        for (int i = 0; i < Height; ++i)
        {
            for (int j = 0; j < Width; ++j)
            {
                if (i == 0 || i == Height - 1 || j == 0 || j == Width - 1) 
                {
                    if (X + j >= 0 && X + j < Canvas.Width &&
                        Y + i >= 0 && Y + i < Canvas.Height)
                    {
                        canvas.CanvasData[Y + i][X + j] = Canvas.FillChar;
                    }
                }
            }
        }
    }

    public override void Fill(Canvas canvas)
    {
        for (int i = 0; i < Height; ++i)
        {
            for (int j = 0; j < Width; ++j)
            {
                if (X + j >= 0 && X + j < Canvas.Width &&
                    Y + i >= 0 && Y + i < Canvas.Height)
                {
                    canvas.CanvasData[Y + i][X + j] = Canvas.FillChar;
                }
            }
        }
    }

    public override void Move(int deltaX, int deltaY)
    {
        
        X += deltaX;
        Y += deltaY;
    }
    public override void Erase(Canvas canvas)
    {
        for (int i = 0; i < Height; ++i)
        {
            for (int j = 0; j < Width; ++j)
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