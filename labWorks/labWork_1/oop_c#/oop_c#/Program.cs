using System;
using System.Drawing;

class Program
{
    static void Main(string[] args)
    {
        Canvas canvas = new Canvas();
        ActionManager actionManager = new ActionManager();

        // Example
        Shape circle = new Circle(40, 15, 5);
        actionManager.ExecuteAction(new AddShapeAction(canvas, circle));

        Shape rectangle = new Rectangle(20, 10, 10, 8);
        actionManager.ExecuteAction(new AddShapeAction(canvas, rectangle));

        Shape triangle = new Triangle(60, 25, 12, 6);
        actionManager.ExecuteAction(new AddShapeAction(canvas, triangle));

        canvas.Draw();

        Console.WriteLine("\nMoving Rectangle...");
        actionManager.ExecuteAction(new MoveShapeAction(canvas, rectangle, rectangle.X, rectangle.Y, rectangle.X + 5, rectangle.Y + 2));
        canvas.Draw();

        Console.WriteLine("\nSaving canvas...");
        canvas.SaveToFile("canvas.txt");

        Console.WriteLine("\nClearing canvas...");
        canvas.Clear();
        canvas.Draw();

        Console.WriteLine("\nLoading canvas...");
        canvas.LoadFromFile("canvas.txt");
        canvas.Draw();

        Console.WriteLine("\nUndoing Move...");
        actionManager.Undo();
        canvas.Draw();

        Console.WriteLine("\nRedoing Move...");
        actionManager.Redo();
        canvas.Draw();
    }
}