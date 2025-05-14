class RemoveShapeAction : Action
{
    private Shape shape;

    public RemoveShapeAction(Canvas canvas, Shape shape) : base(canvas)
    {
        this.shape = shape;
    }

    public override void Execute()
    {
        canvas.RemoveShape(shape);
    }

    public override void Undo()
    {
        canvas.AddShape(shape);
    }
}