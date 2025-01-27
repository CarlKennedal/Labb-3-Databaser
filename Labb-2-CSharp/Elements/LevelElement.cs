using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public abstract class LevelElement
{
    public abstract Position Position { get; set; }
    public abstract char Type { get; set; }
    public abstract ConsoleColor Color { get; set; }
    public bool IsColliding { get; set; }
    public bool IsVisible { get; set; }
    [BsonIgnore]
    public LevelData LevelData { get; init; }
    public void Draw(LevelData level)
    {
        Console.ForegroundColor = Color;
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(Type);
    }
}