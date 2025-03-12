using System;
using System.Drawing;
using System;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        Canvas canvas = new Canvas();
        ActionManager actionManager = new ActionManager();

        while (true)
        {
            

            Console.WriteLine("\nChoose an action:");
            Console.WriteLine("1. Create Shape");
            Console.WriteLine("2. Remove Shape");
            Console.WriteLine("3. Move Shape");
            Console.WriteLine("4. Fill Shape");
            Console.WriteLine("5. Save to File");
            Console.WriteLine("6. Load from File");
            Console.WriteLine("7. Undo");
            Console.WriteLine("8. Redo");
            Console.WriteLine("9. Clear Canvas");
            Console.WriteLine("10. Draw Canvas");
            Console.WriteLine("11. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": 
                    CreateShape(canvas, actionManager);
                    break;
                case "2": 
                    RemoveShape(canvas, actionManager);
                    break;
                case "3": 
                    MoveShape(canvas, actionManager);
                    break;
                case "4": 
                    FillShape(canvas, actionManager);
                    break;
                case "5":
                    SaveToFile(canvas);
                    break;
                case "6": 
                    LoadFromFile(canvas);
                    break;
                case "7": 
                    actionManager.Undo();
                    canvas.Draw();
                    break;
                case "8": 
                    actionManager.Redo();
                    canvas.Draw(); 
                    break;
                case "9": 
                    ClearCanvas(canvas, actionManager);
                    break;
                case "10": 
                    canvas.Draw();
                    break;
                case "11":
                    Console.WriteLine("Exiting program.");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            
        }
    }

    static void CreateShape(Canvas canvas, ActionManager actionManager)
    {
        Console.WriteLine("Choose a shape to create:");
        Console.WriteLine("1. Circle");
        Console.WriteLine("2. Triangle");
        Console.WriteLine("3. Rectangle");
        Console.Write("Enter your choice: ");
        string shapeChoice = Console.ReadLine();

        switch (shapeChoice)
        {
            case "1": 
                Console.Write("Enter X coordinate: ");
                if (!int.TryParse(Console.ReadLine(), out int circleX))
                {
                    Console.WriteLine("Invalid X coordinate.");
                    return;
                }

                Console.Write("Enter Y coordinate: ");
                if (!int.TryParse(Console.ReadLine(), out int circleY))
                {
                    Console.WriteLine("Invalid Y coordinate.");
                    return;
                }

                Console.Write("Enter radius: ");
                if (!int.TryParse(Console.ReadLine(), out int radius))
                {
                    Console.WriteLine("Invalid radius.");
                    return;
                }

                Shape circle = new Circle(circleX, circleY, radius);
                IAction addCircleAction = new AddShapeAction(canvas, circle);
                actionManager.ExecuteAction(addCircleAction);
                canvas.Draw(); // Redraw after adding shape
                break;
            case "2": 
                Console.Write("Enter X coordinate: ");
                if (!int.TryParse(Console.ReadLine(), out int triangleX))
                {
                    Console.WriteLine("Invalid X coordinate.");
                    return;
                }

                Console.Write("Enter Y coordinate: ");
                if (!int.TryParse(Console.ReadLine(), out int triangleY))
                {
                    Console.WriteLine("Invalid Y coordinate.");
                    return;
                }

                Console.Write("Enter the first side: ");
                if (!int.TryParse(Console.ReadLine(), out int side1))
                {
                    Console.WriteLine("Invalid.");
                    return;
                }

                Console.Write("Enter the second side: ");
                if (!int.TryParse(Console.ReadLine(), out int side2))
                {
                    Console.WriteLine("Invalid.");
                    return;
                }
                Console.Write("Enter the third side: ");
                if (!int.TryParse(Console.ReadLine(), out int side3))
                {
                    Console.WriteLine("Invalid.");
                    return;
                }

                Shape triangle = new Triangle(triangleX, triangleY, side1, side2, side3);
                IAction addTriangleAction = new AddShapeAction(canvas, triangle);
                actionManager.ExecuteAction(addTriangleAction);
                canvas.Draw(); 
                break;
            case "3":
                Console.Write("Enter X coordinate: ");
                if (!int.TryParse(Console.ReadLine(), out int rectangleX))
                {
                    Console.WriteLine("Invalid X coordinate.");
                    return;
                }

                Console.Write("Enter Y coordinate: ");
                if (!int.TryParse(Console.ReadLine(), out int rectangleY))
                {
                    Console.WriteLine("Invalid Y coordinate.");
                    return;
                }

                Console.Write("Enter the width: ");
                if (!int.TryParse(Console.ReadLine(), out int Width))
                {
                    Console.WriteLine("Invalid  width.");
                    return;
                }

                Console.Write("Enter the height: ");
                if (!int.TryParse(Console.ReadLine(), out int Height))
                {
                    Console.WriteLine("Invalid height.");
                    return;
                }

                Shape rectangle = new Rectangle(rectangleX, rectangleY, Width, Height);
                IAction addRectangleAction = new AddShapeAction(canvas, rectangle);
                actionManager.ExecuteAction(addRectangleAction);
                canvas.Draw(); 
                break;
            default:
                Console.WriteLine("Invalid shape choice.");
                break;
        }
    }

    static void RemoveShape(Canvas canvas, ActionManager actionManager)
    {
        if (canvas.Shapes.Count == 0)
        {
            Console.WriteLine("The canvas is empty. There are no shapes to remove.");
            return;
        }

        Console.WriteLine("Available Shapes:");
        for (int i = 0; i < canvas.Shapes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {canvas.Shapes[i].GetType().Name} at ({canvas.Shapes[i].X}, {canvas.Shapes[i].Y})");
        }

        Console.Write("Enter the number of the shape to remove: ");
        if (!int.TryParse(Console.ReadLine(), out int shapeNumber) || shapeNumber < 1 || shapeNumber > canvas.Shapes.Count)
        {
            Console.WriteLine("Invalid shape number.");
            return;
        }

        Shape shapeToRemove = canvas.Shapes[shapeNumber - 1];
        IAction removeShapeAction = new RemoveShapeAction(canvas, shapeToRemove);
        actionManager.ExecuteAction(removeShapeAction);
        canvas.Draw(); // Redraw after removing shape
    }

    static void MoveShape(Canvas canvas, ActionManager actionManager)
    {
        if (canvas.Shapes.Count == 0)
        {
            Console.WriteLine("The canvas is empty. There are no shapes to move.");
            return;
        }

        Console.WriteLine("Available Shapes:");
        for (int i = 0; i < canvas.Shapes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {canvas.Shapes[i].GetType().Name} at ({canvas.Shapes[i].X}, {canvas.Shapes[i].Y})");
        }

        Console.Write("Enter the number of the shape to move: ");
        if (!int.TryParse(Console.ReadLine(), out int shapeNumber) || shapeNumber < 1 || shapeNumber > canvas.Shapes.Count)
        {
            Console.WriteLine("Invalid shape number.");
            return;
        }

        Shape shapeToMove = canvas.Shapes[shapeNumber - 1];
        int oldX = shapeToMove.X;
        int oldY = shapeToMove.Y;


        Console.Write("Enter new X coordinate: ");
        if (!int.TryParse(Console.ReadLine(), out int newX))
        {
            Console.WriteLine("Invalid X coordinate.");
            return;
        }

        Console.Write("Enter new Y coordinate: ");
        if (!int.TryParse(Console.ReadLine(), out int newY))
        {
            Console.WriteLine("Invalid Y coordinate.");
            return;
        }

        int deltaX = newX - shapeToMove.X;
        int deltaY = newY - shapeToMove.Y;

        shapeToMove.Move(deltaX, deltaY);

        canvas.RedrawAll();
        canvas.Draw();

      
    }

    static void FillShape(Canvas canvas, ActionManager actionManager)
    {
        if (canvas.Shapes.Count == 0)
        {
            Console.WriteLine("The canvas is empty. There are no shapes to fill.");
            return;
        }

        Console.WriteLine("Available Shapes:");
        for (int i = 0; i < canvas.Shapes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {canvas.Shapes[i].GetType().Name} at ({canvas.Shapes[i].X}, {canvas.Shapes[i].Y})");
        }

        Console.Write("Enter the number of the shape to fill: ");
        if (!int.TryParse(Console.ReadLine(), out int shapeNumber) || shapeNumber < 1 || shapeNumber > canvas.Shapes.Count)
        {
            Console.WriteLine("Invalid shape number.");
            return;
        }

        Shape shapeToFill = canvas.Shapes[shapeNumber - 1];
        IAction fillShapeAction = new FillShapeAction(canvas, shapeToFill);
        actionManager.ExecuteAction(fillShapeAction);
        canvas.Draw(); // Redraw after filling shape
    }

    static void SaveToFile(Canvas canvas)
    {
        Console.Write("Enter filename to save: ");
        string filename = Console.ReadLine();
        canvas.SaveToFile(filename);
    }

    static void LoadFromFile(Canvas canvas)
    {
        Console.Write("Enter filename to load: ");
        string filename = Console.ReadLine();
        canvas.LoadFromFile(filename);
        canvas.Draw(); // Redraw after loading
    }

    static void ClearCanvas(Canvas canvas, ActionManager actionManager)
    {
        // For clear canvas, it is necessary to create new action
        ClearCanvasAction clearCanvasAction = new ClearCanvasAction(canvas);
        actionManager.ExecuteAction(clearCanvasAction);
        canvas.Draw(); // Redraw after clear
    }

    // Class for clear canvas action
    class ClearCanvasAction : Action
    {
        private Canvas canvas;
        private List<Shape> shapes; // store shape list for undo action
        private char[][] canvasData; // store canvasData for undo action

        public ClearCanvasAction(Canvas canvas) : base(canvas)
        {
            this.canvas = canvas;
            this.shapes = new List<Shape>(canvas.Shapes);  // copy shape list
            this.canvasData = new char[Canvas.Height][]; // copy canvas data
            for (int i = 0; i < Canvas.Height; ++i)
            {
                canvasData[i] = new char[Canvas.Width];
                Array.Copy(canvas.CanvasData[i], canvasData[i], Canvas.Width);
            }
        }

        public override void Execute()
        {
            canvas.Clear();
        }

        public override void Undo()
        {
            // Restore previous state of the canvas
            canvas.Shapes = shapes;
            canvas.CanvasData = canvasData;
            foreach (var shape in canvas.Shapes)
            {
                shape.Draw(canvas);
            }
        }
    }
}