using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


public abstract class LivingElement : LevelElement
{
    public abstract int HealthPoints { get; set; }
    public abstract string Name { get; set; }
    public abstract Dice attackDice { get; set; }
    public abstract Dice defenseDice { get; set; }
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
        if (this is Player && collidingElement is Enemy)
        {
            CombatHandler combatHandler = new CombatHandler(LevelData);
            combatHandler.Attack(this, collidingElement as LivingElement );
        }
        else if (this is Enemy && collidingElement is Player)
        {
            CombatHandler combatHandler = new CombatHandler(LevelData);
            combatHandler.Attack(this, collidingElement as LivingElement);
        }
    }
    public abstract void Update();
}
