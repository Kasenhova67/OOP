class FillShapeAction : Action
{
    private Shape shape;

    public FillShapeAction(Canvas canvas, Shape shape) : base(canvas)
    {
        this.shape = shape;
    }

    public override void Execute()
    {
        canvas.FillShape(shape);
    }

    public override void Undo()
    {
        shape.Erase(canvas);
    }
  }
