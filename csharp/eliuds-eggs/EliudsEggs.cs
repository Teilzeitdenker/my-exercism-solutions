public static class EliudsEggs
{
    public static int EggCount(int encodedCount)
    {
        if (encodedCount < 0) throw new ArgumentOutOfRangeException("input must be non-negative");
        if (encodedCount == 0) return 0;
        return (encodedCount & 1) + EggCount(encodedCount >> 1);
    }
}
