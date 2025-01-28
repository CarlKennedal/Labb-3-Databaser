using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[BsonDiscriminator("Wall")]
class Wall : LevelElement
{
    public override Position Position { get; set; }
    public override ConsoleColor Color { get; set; } = ConsoleColor.White;
    public override char Type { get; set; } = '#';
    public override string Name { get; set; } = "Wall";
    public override Dice? attackDice { get; set; }
    public override Dice? defenseDice { get; set; }

    public override void Update()
    {
        throw new NotImplementedException();
    }
}
