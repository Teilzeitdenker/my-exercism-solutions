public static class EliudsEggs
{
    public static int EggCount(int encodedCount) => encodedCount switch
    {
        0 => 0,
        _ => (encodedCount & 1) + EggCount(encodedCount >> 1)
    };
}
