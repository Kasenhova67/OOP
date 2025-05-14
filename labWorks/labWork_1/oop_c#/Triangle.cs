class Triangle : Shape
{
    public int Side1 { get; set; }
    public int Side2 { get; set; }
    public int Side3 { get; set; }

    public double X1 { get; set; }
    public double Y1 { get; set; }
    public double X2 { get; set; }
    public double Y2 { get; set; }
    public double X3 { get; set; }
    public double Y3 { get; set; }

    public Triangle(int x, int y, int side1, int side2, int side3)
    {
        X = x;
        Y = y;
        Side1 = side1;
        Side2 = side2;
        Side3 = side3;

        if (!IsValidTriangle(Side1, Side2, Side3))
        {
            throw new ArgumentException("Invalid triangle sides: The sum of any two sides must be greater than the third side.");
        }

        CalculateVertices();
    }

    private bool IsValidTriangle(int a, int b, int c)
    {
        return (a + b > c) && (a + c > b) && (b + c > a);
    }

    private void CalculateVertices()
    {
        X1 = X;
        Y1 = Y;
        X2 = X + Side1;
        Y2 = Y;

        double cosA = (Math.Pow(Side1, 2) + Math.Pow(Side2, 2) - Math.Pow(Side3, 2)) / (2.0 * Side1 * Side2);
        double angleA = Math.Acos(cosA);

  
        X3 = X + Side2 * Math.Cos(angleA);
        Y3 = Y + Side2 * Math.Sin(angleA);
    }

    public override void Draw(Canvas canvas)
    {
        DrawLine(canvas, (int)Math.Round(X1), (int)Math.Round(Y1), (int)Math.Round(X2), (int)Math.Round(Y2));
        DrawLine(canvas, (int)Math.Round(X2), (int)Math.Round(Y2), (int)Math.Round(X3), (int)Math.Round(Y3));
        DrawLine(canvas, (int)Math.Round(X3), (int)Math.Round(Y3), (int)Math.Round(X1), (int)Math.Round(Y1));
    }


    public override void Fill(Canvas canvas)
    {
        Draw(canvas);
    }

    public override void Erase(Canvas canvas)
    {
        char oldChar = Canvas.FillChar;

        Draw(canvas);
    }

    public override void Move(int deltaX, int deltaY)
    {
       
        X += deltaX;
        Y += deltaY;

        X1 += deltaX;
        Y1 += deltaY;
        X2 += deltaX;
        Y2 += deltaY;
        X3 += deltaX;
        Y3 += deltaY;
    }
    private void DrawLine(Canvas canvas, int x0, int y0, int x1, int y1)
    {
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = (dx > dy ? dx : -dy) / 2, e2;

        while (true)
        {
            if (x0 >= 0 && x0 < Canvas.Width && y0 >= 0 && y0 < Canvas.Height)
            {
                canvas.CanvasData[y0][x0] = Canvas.FillChar;
            }
            if (x0 == x1 && y0 == y1) break;
            e2 = err;
            if (e2 > -dx) { err -= dy; x0 += sx; }
            if (e2 < dy) { err += dx; y0 += sy; }
        }
    }
}