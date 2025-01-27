using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Wall : LevelElement
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public override Position Position { get; set; }
    public override ConsoleColor Color { get; set; } = ConsoleColor.White;
    public override char Type { get; set; } = '#';
}
