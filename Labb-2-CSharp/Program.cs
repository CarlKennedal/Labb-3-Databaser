using Labb_2_CSharp;
using MongoDB.Driver;
using System.Linq;
using System.Reflection.Emit;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

StartScreen();


static void GameLoop(LevelData levelEtt, Player player)
{
    player = levelEtt.elements.First(e => e is Player) as Player;
    player.IsVisible = true;
    CollisionHandler collisionHandler = new CollisionHandler(levelEtt);

    Console.SetCursorPosition(0, 23);
    Console.WriteLine("Press 'K' to save game, 'L' to load.");
    Console.SetCursorPosition(0, 24);
    Console.WriteLine("Press ESC to exit.");
    RenderLevel(levelEtt, player);
    Console.SetCursorPosition(0, 0);
    Console.Write($"Player: {player.HealthPoints} HP, {player.attackDice.numberOfDice}d{player.attackDice.sidesPerDice}+{player.attackDice.modifier} ATK, {player.defenseDice.numberOfDice}d{player.defenseDice.sidesPerDice}+{player.defenseDice.modifier} DEF.\n Has survived a total of 0 turns!");


    while (true)
    {
        ConsoleKey keyPressed = Console.ReadKey(intercept: true).Key;
        levelEtt.ConsoleKey = keyPressed;

        if (keyPressed == ConsoleKey.K)
        {
            SaveGame(levelEtt);
        }
        else if (keyPressed == ConsoleKey.Escape)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            StartScreen();
        }
        else if (keyPressed == ConsoleKey.L)
        {
            Console.Clear();
            LoadGame(ref levelEtt, ref player);
            collisionHandler.UpdateLevelData(levelEtt);
            Console.SetCursorPosition(0, 23);
            Console.WriteLine("Press 'K' to save game or 'L' to load previous game.");
            RenderLevel(levelEtt, player);
        }
        else
        {
            UpdateGame(levelEtt, player);
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

static void LoadGame(ref LevelData levelEtt, ref Player player)
{
    levelEtt = new LevelData();
    levelEtt.damageOutput = -1;
    levelEtt.elements.Clear();

    var mongoContext = new MongoDBContext("CarlKennedal");
    var elementsCollection = mongoContext.GetCollection<LevelElement>("LevelElements");

    var savedElements = elementsCollection.Find(_ => true).ToList();

    foreach (var element in savedElements)
    {
        if (element is Player loadedPlayer)
        {
            levelEtt.elements.Add(loadedPlayer);
            levelEtt.player = loadedPlayer;
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
    player = levelEtt.elements.FirstOrDefault(e => e is Player) as Player;
    RenderLevel(levelEtt, player);
}
static void UpdateGame(LevelData levelEtt, Player player)
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
static void StartScreen()
{
    Console.WriteLine("  ·▄▄▄▄  ▄• ▄▌ ▐ ▄  ▄▄ • ▄▄▄ .       ▐ ▄      ▄▄· ▄▄▄   ▄▄▄· ▄▄▌ ▐ ▄▌▄▄▌  ▄▄▄ .▄▄▄  \r\n  ██▪ ██ █▪██▌•█▌▐█▐█ ▀ ▪▀▄.▀·▪     •█▌▐█    ▐█ ▌▪▀▄ █·▐█ ▀█ ██· █▌▐███•  ▀▄.▀·▀▄ █·\r\n  ▐█· ▐█▌█▌▐█▌▐█▐▐▌▄█ ▀█▄▐▀▀▪▄ ▄█▀▄ ▐█▐▐▌    ██ ▄▄▐▀▀▄ ▄█▀▀█ ██▪▐█▐▐▌██▪  ▐▀▀▪▄▐▀▀▄ \r\n  ██. ██ ▐█▄█▌██▐█▌▐█▄▪▐█▐█▄▄▌▐█▌.▐▌██▐█▌    ▐███▌▐█•█▌▐█ ▪▐▌▐█▌██▐█▌▐█▌▐▌▐█▄▄▌▐█•█▌\r\n  ▀▀▀▀▀•  ▀▀▀ ▀▀ █▪·▀▀▀▀  ▀▀▀  ▀█▄▀▪▀▀ █▪    ·▀▀▀ .▀  ▀ ▀  ▀  ▀▀▀▀ ▀▪.▀▀▀  ▀▀▀ .▀  ▀\r\n\r\n\t\t  Press 'Space' to start or 'L' to load your latest save.\r\n\t\t\t\tPress 'Escape' to exit.");
   
    ConsoleKey keyPressed;
    do
    {
        keyPressed = Console.ReadKey(intercept: true).Key;
    }
    while (keyPressed != ConsoleKey.Spacebar && keyPressed != ConsoleKey.L && keyPressed != ConsoleKey.Escape);

    LevelData levelEtt = new LevelData();
    Player player = null;

    if (keyPressed == ConsoleKey.Spacebar)
    {
        levelEtt.Load("Level1.txt");
        player = levelEtt.elements.FirstOrDefault(e => e is Player) as Player;
        if (player != null) player.IsVisible = true;
        Console.Clear();
    }
    else if (keyPressed == ConsoleKey.L)
    {
        Console.Clear();
        Console.WriteLine("Loading previous game...");
        LoadGame(ref levelEtt, ref player); // 👈 Pass player reference
    }
    else
    {
        Console.Clear();
        Environment.Exit(0);
    }
    if (player != null)
    {
        GameLoop(levelEtt, player);
    }
    else
    {
        Console.WriteLine("No player found. Exiting...");
        Environment.Exit(0);
    }
}
static void RenderLevel(LevelData levelEtt, Player player)
{
    RenderDistance(player, levelEtt);
    foreach (LevelElement draw in levelEtt.elements)
    {
        if (draw.IsVisible)
        {
            draw.Draw(levelEtt);
        }
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