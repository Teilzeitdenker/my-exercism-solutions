using System;

public static class BinarySearch
{
    public static int Find(int[] input, int value)
    {
        if (input.Length == 0) return -1;
        int middle = input.Length / 2;
        if (input[middle] == value) 
            return middle;
        else if (input[middle] > value) 
            return Find(input[0..middle], value);
        else 
            return Find(input[(middle + 1)..], value) == -1 ? -1 : middle + 1 + Find(input[(middle + 1)..], value);
    }
}