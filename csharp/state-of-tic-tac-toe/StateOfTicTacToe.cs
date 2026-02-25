public enum State
{
    Win,
    Draw,
    Ongoing,
    Invalid
}

public class TicTacToe
{
    private readonly char[][] board;
    private readonly int xCount;
    private readonly int oCount;
    private readonly bool xWins;
    private readonly bool oWins;
    private readonly State state;

    public TicTacToe(string[] rows)
    {
        board = rows.Select(row => row.ToCharArray()).ToArray();
        xCount = board.Sum(row => row.Count(cell => cell == 'X'));
        oCount = board.Sum(row => row.Count(cell => cell == 'O'));
        xWins = CheckWin('X');
        oWins = CheckWin('O');
        state = DeterminateState();
    }

    public State State => state;
    
    private bool CheckWin(char player)
    {
        // Check rows and columns
        for (int i = 0; i < 3; i++)
        {
            if (board[i].All(cell => cell == player)) return true;
            if (board.All(row => row[i] == player)) return true;
        }
        // Check diagonals
        if (board[0][0] == player && board[1][1] == player && board[2][2] == player) return true;
        if (board[0][2] == player && board[1][1] == player && board[2][0] == player) return true;
        return false;
    }

    private State DeterminateState()
    {
        if (xCount < oCount || xCount > oCount + 1) return State.Invalid;
        if (xWins && oWins) return State.Invalid;
        if (xWins) return State.Win;
        if (oWins) return State.Win;
        if (xCount + oCount == 9) return State.Draw;
        return State.Ongoing;
    }
}
