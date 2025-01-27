using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Direction
{
    UP,
    DOWN,
    RIGHT,
    LEFT,
};

class Rat : Enemy
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public override char Type { get; set; } = 'r';
    public override Position Position { get; set; }
    public override Dice attackDice { get; set; } = new Dice(1, 6, 3);
    public override Dice defenseDice { get; set; } = new Dice(1, 6, 1);
    public override string Name { get; set; } = "Rat";
    public override int HealthPoints { get; set; } = 10;
    public override ConsoleColor Color { get; set; } = ConsoleColor.Red;


    public override void Update()
    {
        Random randomDirection = new Random();
        Direction randomDir = (Direction)randomDirection.Next(0, 4);
        switch (randomDir)
        {
            case Direction.UP:
                Move(y: -1);
                break;
            case Direction.RIGHT:
                Move(x: +1);
                break;
            case Direction.DOWN:
                Move(y: +1);
                break;
            case Direction.LEFT:
                Move(x: -1);
                break;
        }
    }
}


