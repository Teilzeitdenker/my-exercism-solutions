public static class SquareRoot
{
    public static int Root(int number) => ISqrt(number, Seed(number));

    private static int Log2(int number)
    {
        int result = 0;
        while (number > 1)
        {
            number >>= 1;
            result++;
        }
        return result;
    }

    private static int Seed(int number) => 1 << ((Log2(number) / 2) + 1);

    private static int ISqrt(int number, int seed)
    {
        int next = (seed + number / seed) >> 1;
        if (next >= seed)
            return seed;
        return ISqrt(number, next);
    }
}
