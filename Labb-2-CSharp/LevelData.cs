using Labb_2_CSharp;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public class LevelData
{
    public int damageOutput { get; set; } = -1;
    public List<LevelElement> elements { get; set; } = new List<LevelElement>();
    public static Player leveldataPlayer;
    public ConsoleKey? ConsoleKey {  get; set; } = System.ConsoleKey.None;
    public void Load(string fileName)
    {
        using (StreamReader reader = new StreamReader(fileName))
        {
            string characters;
            int indexOfY = 4;
           
            while ((characters = reader.ReadLine()) != null)
            {
            int indexOfX = 0;
                foreach (char c in characters)
                {
                    switch (c)
                    {
                        case '@':
                            var player = leveldataPlayer = new Player() { LevelData = this };
                            player.Position = new Position(indexOfX, indexOfY);
                            player.HealthPoints = 100;
                            elements.Add(player);
                            break;

                        case '#':
                            var wall = new Wall() { LevelData = this };
                            wall.Position = new Position(indexOfX, indexOfY);
                            elements.Add(wall);
                            break;

                        case 'r':
                            var rat = new Rat() { LevelData = this };
                            rat.HealthPoints = 10;
                            rat.Position = new Position(indexOfX, indexOfY);
                            elements.Add(rat);
                            break;

                        case 's':
                            var snake = new Snake() { LevelData = this };
                            snake.Position = new Position(indexOfX, indexOfY);
                            snake.HealthPoints = 25;
                            elements.Add(snake);
                            break;
                    }
                    indexOfX++;
                }
                indexOfY++;
            }
        }
    }
}
