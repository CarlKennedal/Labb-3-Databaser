using Labb_2_CSharp;
using MongoDB.Driver;
using System.Linq;
using System.Reflection.Emit;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

LevelData levelEtt = new LevelData();
levelEtt.Load("Level1.txt");
Player player = levelEtt.elements.First(e => e is Player) as Player;
player.IsVisible = true;
CollisionHandler collisionHandler = new CollisionHandler(levelEtt);
Console.SetCursorPosition(0, 23);
Console.WriteLine("Press 'K' to save game or 'L' to load previous game.");

while (true)
{
    var keyPressed = Console.ReadKey(intercept: true).Key;
    levelEtt.ConsoleKey = keyPressed;
    if (keyPressed == ConsoleKey.K)
    {
        SaveGame(levelEtt);
    }
    else if (keyPressed == ConsoleKey.L)
    {
        Console.Clear();
        LoadGame(ref levelEtt);
        collisionHandler.UpdateLevelData(levelEtt);
        Console.SetCursorPosition(0, 23);
        Console.WriteLine("Press 'K' to save game or 'L' to load previous game.");
        RenderDistance(player, levelEtt);
        foreach (LevelElement draw in levelEtt.elements)
        {
            if (draw.IsVisible == true)
            {
                draw.Draw(levelEtt);
            }
        }
    }
    else
    {
        RenderDistance(player, levelEtt);
        foreach (LevelElement mobs in levelEtt.elements.ToList())
        {
            if (mobs is not Wall)
            {
                mobs.Update();
            }
            if (mobs.IsVisible == true)
            {
                mobs.Draw(levelEtt);
            }
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
            if (otherElement is Rat || otherElement is Snake)
            {
                if (otherElement is Rat rat)
                {
                    rat.IsVisible = false;
                }
                else if (otherElement is Snake snake)
                {
                    snake.IsVisible = false;
                }
            }
        }
    }
}
static void SaveGame(LevelData levelEtt)
{
    var mongoContext = new MongoDBContext("CarlKennedal");

    var elementsCollection = mongoContext.GetCollection<LevelElement>("LevelElements");

    elementsCollection.DeleteMany(_ => true);

    var allElements = levelEtt.elements.Select(e =>
    {
        e.Id = Guid.NewGuid().ToString();
        e.Type = e switch
        {
            Player => '@',
            Wall => '#',
            Rat => 'r',
            Snake => 's',

        };
        return e;
    }).ToList();

    if (allElements.Any())
    {
        elementsCollection.InsertMany(allElements);
    }
}

static void LoadGame(ref LevelData levelEtt)
{
    levelEtt = new LevelData();
    levelEtt.damageOutput = -1;
    levelEtt.elements.Clear();

    var mongoContext = new MongoDBContext("CarlKennedal");
    var elementsCollection = mongoContext.GetCollection<LevelElement>("LevelElements");


    var savedElements = elementsCollection.Find(_ => true).ToList();

    foreach (var element in savedElements)
    {
        if (element is Player player)
        {
            levelEtt.elements.Add(player);
        }
        else if (element is Wall wall)
        {
            levelEtt.elements.Add(wall);

        }
        else if (element is Rat rat)
        {
            levelEtt.elements.Add(rat);
        }
        else if (element is Snake snake)
        {
            levelEtt.elements.Add(snake);
        }
    }
    foreach (var element in levelEtt.elements)
    {
        element.LevelData = levelEtt;
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
    public void UpdateLevelData(LevelData level)
    {
        this.level = level;
    }
}
public class CombatHandler
{
    private LevelData level;
    public CombatHandler(LevelData level)
    {
        this.level = level;
    }
    public void Attack(LevelElement attacker, LevelElement defender)
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
    public void CounterAttack(LevelElement attacker, LevelElement defender)
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