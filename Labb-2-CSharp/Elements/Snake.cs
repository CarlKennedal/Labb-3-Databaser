using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[BsonDiscriminator("Snake")]
class Snake : LevelElement
{
    public override char Type { get; set; } = 's';
    public override Position Position { get; set; }
    public override string Name { get; set; } = "Snake";
    public override Dice attackDice { get; set; } = new Dice(3, 4, 2);
    public override Dice defenseDice { get; set; } = new Dice(1, 8, 0);
    public override ConsoleColor Color { get; set; } = ConsoleColor.Green;

    public override void Update()
    {
        MoveSnake(this, LevelData.leveldataPlayer);
    }
    public void MoveSnake(Snake snake, Player player)
    {
        Console.ForegroundColor = ConsoleColor.White;
        int distance = snake.Position.DistanceTo(player.Position);
        if (distance <= 2)
        {
            if (player.Position.Y <= snake.Position.Y - 2)
            {
                Move(y: +1);
            }
            else if (player.Position.X <= snake.Position.X - 2)
            {
                Move(x: +1);
            }
            else if (player.Position.Y <= snake.Position.Y + 2)
            {
                Move(y: -1);
            }
            else if (player.Position.X <= snake.Position.X + 2)
            {
                Move(x: -1);
            }
        }
        if (distance >= 3)
        {
            Move(y: +0);
        }
    }
}

