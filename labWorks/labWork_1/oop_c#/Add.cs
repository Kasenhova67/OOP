class AddShapeAction : Action
{
    private Shape shape;

    public AddShapeAction(Canvas canvas, Shape shape) : base(canvas)
    {
        this.shape = shape;
    }

    public override void Execute()
    {
        canvas.AddShape(shape);
    }

    public override void Undo()
    {
        canvas.RemoveShape(shape);
    }
}