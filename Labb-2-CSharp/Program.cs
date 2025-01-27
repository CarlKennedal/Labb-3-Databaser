using Labb_2_CSharp;
using MongoDB.Driver;
using System.Linq;
using System.Reflection.Emit;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

LevelData levelEtt = new LevelData();
levelEtt.Load("Level1.txt");
Player player = levelEtt.elements.First(e => e is Player) as Player;
Console.SetCursorPosition(0, 23);
Console.WriteLine("Press 'K' to save game or 'L' to load previous game.");


while (true)
{
    var keyPressed = Console.ReadKey(intercept: true).Key;
    if (keyPressed == ConsoleKey.K)
    {
        SaveGame(levelEtt);
    }
    else if (keyPressed == ConsoleKey.L)
    {
        //    LoadGame();
    }
    else
    {
        RenderDistance(player, levelEtt);
        foreach (LevelElement draw in levelEtt.elements)
        {
            if (draw.IsVisible)
            {
                draw.Draw(levelEtt);
            }
        }
        player.PlayerUpdate(keyPressed);
        foreach (LivingElement mobs in levelEtt.elements.OfType<LivingElement>())
        {
            mobs.Update();
        }
    }
}

static void RenderDistance(Player player, LevelData level)
{
    foreach (var otherElement in level.elements)
    {
        int distance = player.Position.DistanceTo(otherElement.Position);
        if (distance <= 5)
        {
            otherElement.IsVisible = true;
        }
        if (distance >= 5)
        {
            if (otherElement is Enemy)
            {
                var enemy = (Enemy)otherElement;
                enemy.IsVisible = false;
            }
        }
    }
}
static void SaveGame(LevelData levelEtt)
{
    var mongoContext = new MongoDBContext("DungeonCrawlDB");
    var playersCollection = mongoContext.GetCollection<Player>("Players");
    var wallsCollection = mongoContext.GetCollection<Wall>("Walls");
    var ratsCollection = mongoContext.GetCollection<Rat>("Rats");
    var snakesCollection = mongoContext.GetCollection<Snake>("Snakes");

    foreach (var element in levelEtt.elements)
    {
        if (element is Player player)
        {
            playersCollection.InsertOne(player);
        }
        else if (element is Wall wall)
        {
            wallsCollection.InsertOne(wall);
        }
        else if (element is Rat rat)
        {
            ratsCollection.InsertOne(rat);
        }
        else if (element is Snake snake)
        {
            snakesCollection.InsertOne(snake);
        }
    }
}

static void LoadGame(LevelData levelEtt)
{
    var mongoContext = new MongoDBContext("DungeonCrawlDB");
    var playersCollection = mongoContext.GetCollection<Player>("Players");
    var ratsCollection = mongoContext.GetCollection<Rat>("Rats");
    var snakesCollection = mongoContext.GetCollection<Snake>("Snakes");
    var wallsCollection = mongoContext.GetCollection<Wall>("Walls");

    var players = playersCollection.Find(_ => true).ToList();
    var rats = ratsCollection.Find(_ => true).ToList();
    var snakes = snakesCollection.Find(_ => true).ToList();
    var walls = wallsCollection.Find(_ => true).ToList();

    levelEtt.elements.Clear();

    foreach (var player in players)
    {
        levelEtt.elements.Add(player);
    }

    foreach (var rat in rats)
    {
        levelEtt.elements.Add(rat);
    }

    foreach (var snake in snakes)
    {
        levelEtt.elements.Add(snake);
    }

    foreach (var wall in walls)
    {
        levelEtt.elements.Add(wall);
    }
}

public class CollisionHandler
{
    private LevelData level;

    public CollisionHandler(LevelData level)
    {
        this.level = level;
    }

    public LevelElement CollisionCheck(int x, int y)
    {
        foreach (var element in level.elements)
        {
            if (element.Position.X == x && element.Position.Y == y)
            {
                return element;
            }
        }
        return null;
    }
}
public class CombatHandler
{
    private LevelData level;
    public CombatHandler(LevelData level)
    {
        this.level = level;
    }
    public void Attack(LivingElement attacker, LivingElement defender)
    {
        int atack = attacker.attackDice.Throw();
        int defense = defender.defenseDice.Throw();
        int damage = atack - defense;
        if (damage < 1)
        {
            damage = 0;
        }
        attacker.attackDice.ToString(attacker, defender, damage, level);
        if (damage > 0)
        {
            defender.HealthPoints -= damage;
        }
        if (defender.HealthPoints > 0)
        {
            CounterAttack(defender, attacker);
        }
        else if (defender.HealthPoints < 1)
        {
            level.elements.Remove(defender);
        }
    }
    public void CounterAttack(LivingElement attacker, LivingElement defender)
    {
        int atack = attacker.attackDice.Throw();
        int defense = defender.defenseDice.Throw();
        int damage = atack - defense;
        if (damage < 1)
        {
            damage = 0;
        }
        attacker.attackDice.ToString(attacker, defender, damage, level);
        if (damage > 0)
        {
            defender.HealthPoints -= damage;
        }
        else if (defender.HealthPoints < 1)
        {
            level.elements.Remove(defender);
        }
    }

}