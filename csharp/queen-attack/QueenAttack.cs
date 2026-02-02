using System;

public class Queen
{
    public Queen(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public int Row { get; }
    public int Column { get; }
}

public static class QueenAttack
{
    public static bool CanAttack(Queen white, Queen black)
    {
        if (white.Column == black.Column || white.Row == black.Row) return true;
        if (Math.Abs(white.Column - black.Column) == Math.Abs(white.Row - black.Row)) return true;
        return false;
    }

    public static Queen Create(int row, int column)
    {
        if (IsInRange(row) && IsInRange(column))
            return new Queen(row, column);
        else throw new ArgumentOutOfRangeException("Queen is not on the chess board");
    }

    private static bool IsInRange(int n)
    {
        if (n < 0 || n > 7) return false;
        else return true;
    }
}