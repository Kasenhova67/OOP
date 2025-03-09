class MoveShapeAction : Action
{
    private Shape shape;
    private int oldX, oldY, newX, newY;

    public MoveShapeAction(Canvas canvas, Shape shape, int oldX, int oldY, int newX, int newY) : base(canvas)
    {
        this.shape = shape;
        this.oldX = oldX;
        this.oldY = oldY;
        this.newX = newX;
        this.newY = newY;
    }

    public override void Execute()
    {
        shape.Move(newX - oldX, newY - oldY);
        canvas.MoveShape(shape, newX - oldX, newY - newY);
    }

    public override void Undo()
    {
        shape.Move(oldX - newX, oldY - newY);
        canvas.MoveShape(shape, oldX - newX, oldY - newY);
    }
}