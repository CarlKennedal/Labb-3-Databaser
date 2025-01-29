using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

[BsonDiscriminator("Player")]
public class Player : LevelElement
{
    public override char Type { get; set; } = '@';
    public override string Name { get; set; } = "Player";
    public override Dice attackDice { get; set; } = new Dice(2, 6, 2);
    public override Dice defenseDice { get; set; } = new Dice(2, 6, 0);

    public override Position Position { get; set; }
    public override ConsoleColor Color { get; set; } = ConsoleColor.Blue;

    public int renderDistance = 5;
    public int move = 0;
    public int turns = 0;
    public override void Update()
    {
        turns++;
        Console.SetCursorPosition(0, 0);
        Console.Write($"Player: {HealthPoints} HP, {attackDice.numberOfDice}d{attackDice.sidesPerDice}+{attackDice.modifier} ATK, {defenseDice.numberOfDice}d{defenseDice.sidesPerDice}+{defenseDice.modifier} DEF.\n Has survived a total of {turns} turns!");

        if (LevelData != null && LevelData.ConsoleKey.HasValue)
        {
            switch (LevelData.ConsoleKey)
            {
                case ConsoleKey.UpArrow:
                    Move(y: -1);
                    break;
                case ConsoleKey.DownArrow:
                    Move(y: +1);
                    break;
                case ConsoleKey.LeftArrow:
                    Move(x: -1);
                    break;
                case ConsoleKey.RightArrow:
                    Move(x: +1);
                    break;
                default:
                    break;
            }
        }
        else
        {
            Console.WriteLine("LevelData or ConsoleKey is null.");
        }
    }
}
