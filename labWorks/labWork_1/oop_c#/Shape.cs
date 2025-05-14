using System;
abstract class Shape : IDrawable
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsFilled { get; set; } = false; 
    public abstract void Draw(Canvas canvas);
    public abstract void Fill(Canvas canvas);
    public abstract void Erase(Canvas canvas);
    public abstract void Move(int deltaX, int deltaY);
    public void Redraw(Canvas canvas)
    {
        if (IsFilled)
        {
            Fill(canvas); 
        }
        else
        {
            Draw(canvas); 
        }
    }
}