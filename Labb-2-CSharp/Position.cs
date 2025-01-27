using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


public struct Position
{
    private int x;

    public int X
    {
        get { return x; }
        set { x = value; }
    }

    private int y;

    public int Y
    {
        get { return y; }
        set { y = value; }
    }
    public Position(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public int DistanceTo(Position other)
    {
        int posX = other.X - this.X;
        int posY = other.Y - this.Y;
        return (int)Math.Round(Math.Sqrt(posX * posX + posY * posY));
    }
    public int VerticalDistanceTo(Position position) => Math.Abs(position.X - this.X);
    public int HorisontalDistanceTo(Position position) => Math.Abs(position.Y - this.Y);
}