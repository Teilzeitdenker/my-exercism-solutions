using System;
using System.Collections.Generic;

public enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

public class RobotSimulator
{
    public RobotSimulator(Direction direction, int x, int y)
    {
        Direction = direction;
        X = x;
        Y = y;
    }
    public Direction Direction { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public void Move(string instructions)
    {
        foreach (char c in instructions)
        {
            MoveOne(c);
        }
    }
    private void MoveOne(char c)
    {
        switch (c) 
        {
            case 'L':
                TurnLeft();
                break;
            case 'R':
                TurnRight();
                break;
            case 'A':
                Advance();
                break;
            default:
                throw new InvalidOperationException();
        }
    }
    private void TurnLeft()
    {
        Direction = (Direction)( ( (int)Direction + 3 ) % 4 );
    }
    private void TurnRight()
    {
        Direction = (Direction)(((int)Direction + 1) % 4);
    }
    private void Advance()
    {
        (int, int)[] arrOfChanges = new (int, int)[4] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        var change = arrOfChanges[(int)this.Direction];
        X += change.Item1;
        Y += change.Item2; 
    }
}