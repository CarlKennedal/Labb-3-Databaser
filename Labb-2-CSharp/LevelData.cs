using Labb_2_CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;




public class LevelData
{
    public int damageOutput = -1;
    public List<LevelElement> elements { get; set; } = new List<LevelElement>();
    public static Player leveldataPlayer;
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
                            elements.Add(player);
                            break;

                        case '#':
                            var wall = new Wall() { LevelData = this };
                            wall.Position = new Position(indexOfX, indexOfY);
                            elements.Add(wall);
                            break;

                        case 'r':
                            var rat = new Rat() { LevelData = this };
                            rat.Position = new Position(indexOfX, indexOfY);
                            elements.Add(rat);
                            break;

                        case 's':
                            var snake = new Snake() { LevelData = this };
                            snake.Position = new Position(indexOfX, indexOfY);
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
