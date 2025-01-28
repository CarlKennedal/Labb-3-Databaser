using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class Dice
{
    public int numberOfDice;
    public int sidesPerDice;
    public int modifier;
    private LevelData level;
    public Dice(LevelData level)
    {
        this.level = level;
    }
    public Dice(int numberOfDice, int sidesPerDice, int modifier)
    {
        this.numberOfDice = numberOfDice;
        this.sidesPerDice = sidesPerDice;
        this.modifier = modifier;
    }
    public int Throw()
    {
        Random random = new Random();
        int total = 0;

        for (int i = 0; i < numberOfDice; i++)
        {
            total += random.Next(1, sidesPerDice + 1);
        }
        total += modifier;
        return total;
    }
    public void ToString(LevelElement attacker, LevelElement defender, int damage,LevelData level)
    {
        level.damageOutput += 1;
        Console.SetCursorPosition(56, level.damageOutput);
        if (attacker is Rat ||attacker is Snake)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{attacker} attacks {defender} with a roll of {numberOfDice}d{sidesPerDice} + {modifier} dealing {damage} damage.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
        if (attacker is Player)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{attacker} attacks {defender} with a roll of {numberOfDice}d{sidesPerDice} + {modifier} dealing {damage} damage.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}