public static class Camicia
{
    public enum GameStatus { Finished, Loop }
    public record GameResult(GameStatus Status, int Tricks, int Cards);
    private static void TransferAll(this Queue<int> from, Queue<int> to)
    {
        while (from.Count > 0) to.Enqueue(from.Dequeue());
    }
    private static int EncodeCard(string s) => s switch {"J" => 1, "Q" => 2, "K" => 3, "A" => 4, _ => 0};
    private static string GetStateSignature(Queue<int> handA, Queue<int> handB) =>
        $"{string.Concat(handA.Select(Convert.ToChar))}|{string.Concat(handB.Select(Convert.ToChar))}";
    public static GameResult SimulateGame(string[] playerA, string[] playerB)
    {
        var handA = new Queue<int>(playerA.Select(EncodeCard));
        var handB = new Queue<int>(playerB.Select(EncodeCard));
        var pile = new Queue<int>();
        var states = new HashSet<string> { GetStateSignature(handA, handB) };
        // player switches between A to B by multiplication with -1
        int player = 1, tricks = 0, turns = 0, nextCard;
        bool finite = true;

        while (handA.Count > 0 && handB.Count > 0 && finite)
        {
            bool battle = false;
            int cardsToPlay = 1;
            while (cardsToPlay > 0)
            {
                var hand = player == 1 ? handA : handB;
                if (!hand.TryDequeue(out nextCard)) break;
                turns += 1;
                pile.Enqueue(nextCard);
                if (nextCard == 0)
                {
                    if (battle) cardsToPlay -= 1;
                    else player *= -1;
                } 
                else
                {
                    battle = true;
                    cardsToPlay = nextCard;
                    player *= -1;
                }
            }
            tricks += 1;
            player *= -1; // the other player has to take the pile and play on
            pile.TransferAll(player == 1 ? handA : handB);
            if (!states.Add(GetStateSignature(handA, handB))) finite = false;
        }
        GameStatus status = finite ? GameStatus.Finished : GameStatus.Loop;
        return new GameResult(status, tricks, turns);
    }
}
