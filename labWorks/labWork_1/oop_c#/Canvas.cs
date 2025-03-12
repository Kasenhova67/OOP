using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Canvas
{
    public const int Width = 80;
    public const int Height = 30;
    public const char FillChar = '*';

    public char[][] CanvasData { get; set; }

    public Canvas()
    {
        CanvasData = new char[Height][];
        for (int i = 0; i < Height; ++i)
        {
            CanvasData[i] = new char[Width];
            for (int j = 0; j < Width; ++j)
            {
                CanvasData[i][j] = ' ';
            }
        }
    }

    public List<Shape> Shapes { get; set; } = new List<Shape>();

    public void AddShape(Shape shape)
    {
        Shapes.Add(shape);
        shape.Draw(this);
    }

    public void RemoveShape(Shape shape)
    {
        if (shape != null)
        {
            Shapes.Remove(shape);
            RedrawAll();
        }
    }

    public void MoveShape(Shape shape, int newX, int newY)
    {
        if (shape != null)
        {
            shape.Move(newX, newY);
        }
    }

    public void FillShape(Shape shape)
    {
        if (shape != null)
        {
            shape.IsFilled = true;
            shape.Redraw(this);
        }
    }

    public void RedrawAll()
    {
        Clear();
        foreach (var shape in Shapes)
        {
            shape.Redraw(this);
        }
    }

    public void Draw()
    {
        
        Console.Write("+");
        for (int j = 0; j < Width; ++j)
        {
            Console.Write("-");
        }
        Console.WriteLine("+");

        
        for (int i = 0; i < Height; ++i)
        {
            Console.Write("|"); 
            for (int j = 0; j < Width; ++j)
            {
                Console.Write(CanvasData[i][j]);
            }
            Console.WriteLine("|");
        }

       
        Console.Write("+");
        for (int j = 0; j < Width; ++j)
        {
            Console.Write("-");
        }
        Console.WriteLine("+");
    }

    public void Clear()
    {
        for (int i = 0; i < Height; ++i)
        {
            for (int j = 0; j < Width; ++j)
            {
                CanvasData[i][j] = ' ';
            }
        }
    }

public void SaveToFile(string filename)
    {
        try
        {
            using (StreamWriter file = new StreamWriter(filename))
            {
                for (int i = 0; i < Height; ++i)
                {
                    file.WriteLine(new string(CanvasData[i]));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unable to open file for saving: {ex.Message}");
        }
    }

    public void LoadFromFile(string filename)
    {
        Clear();
        try
        {
            using (StreamReader file = new StreamReader(filename))
            {
                for (int i = 0; i < Height; ++i)
                {
                    string line = file.ReadLine();
                    if (line != null && line.Length == Width)
                    {
                        CanvasData[i] = line.ToCharArray();
                    }
                }
            }
            foreach (var shape in Shapes)
            {
                shape.Draw(this);
            }

        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unable to open file for loading: {ex.Message}");
        }
    }
}