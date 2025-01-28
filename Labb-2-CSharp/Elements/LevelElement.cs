using MongoDB.Bson;
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
    [BsonId]
    public string Id { get; set; }
    public abstract Position Position { get; set; }
    public abstract char Type { get; set; }
    public abstract ConsoleColor Color { get; set; }
    public bool IsColliding { get; set; }
    public bool IsVisible { get; set; }
    public int HealthPoints { get; set; }
    public abstract string Name { get; set; }
    public abstract Dice attackDice { get; set; }
    public abstract Dice defenseDice { get; set; }
    [BsonIgnore]
    public LevelData LevelData { get; set; }

    public void Move(int x = 0, int y = 0)
    {
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(' ');
        CollisionHandler collisionHandler = new CollisionHandler(LevelData);
        LevelElement collidingElement = collisionHandler.CollisionCheck(Position.X + x, Position.Y + y);
        if (collidingElement is null)
        {
            Position = new Position { X = Position.X + x, Y = Position.Y + y };
        }
        if (this is Player && collidingElement is Rat)
        {
            CombatHandler combatHandler = new CombatHandler(LevelData);
            combatHandler.Attack(this, collidingElement as LevelElement);
        }
        else if (this is Player && collidingElement is Snake)
        {
            CombatHandler combatHandler = new CombatHandler(LevelData);
            combatHandler.Attack(this, collidingElement as LevelElement);
        }
        else if (this is Rat && collidingElement is Player)
        {
            CombatHandler combatHandler = new CombatHandler(LevelData);
            combatHandler.Attack(this, collidingElement as LevelElement);
        } 
        else if (this is Snake && collidingElement is Player)
        {
            CombatHandler combatHandler = new CombatHandler(LevelData);
            combatHandler.Attack(this, collidingElement as LevelElement);
        }
    }
    public void Draw(LevelData level)
    {
        Console.ForegroundColor = Color;
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(Type);
        Console.ForegroundColor = ConsoleColor.White;
    }
    public abstract void Update();
}