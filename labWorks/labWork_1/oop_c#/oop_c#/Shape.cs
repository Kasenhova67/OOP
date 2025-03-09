using System;

// Shape class (Abstract)
abstract class Shape : IDrawable
{
    public int X { get; set; }
    public int Y { get; set; }

    public abstract void Draw(Canvas canvas);
    public abstract void Fill(Canvas canvas);
    public abstract void Erase(Canvas canvas);

    public virtual void Move(int xOffset, int yOffset)
    {
        X += xOffset;
        Y += yOffset;
    }
}