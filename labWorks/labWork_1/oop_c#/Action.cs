abstract class Action : IAction
{
    protected Canvas canvas;
    public abstract void Execute();
    public abstract void Undo();
    public Action(Canvas canvas)
    {
        this.canvas = canvas;
    }
}