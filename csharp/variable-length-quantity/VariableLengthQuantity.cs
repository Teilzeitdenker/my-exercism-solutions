using System;
using System.Collections.Generic;

public static class VariableLengthQuantity
{
    public static uint[] Encode(uint[] numbers)
    {
        var result = new List<uint>();
        for (int i = 0; i < numbers.Length; ++i)
        {
            var intermediate = new List<uint>();
            uint v = numbers[i];
            uint lastEntry = (byte)(v & 0x7F); // get rid of continuation bit for last entry
            intermediate.Add(lastEntry);
            v >>= 7;
            while (v > 0)
            {
                uint nextEntry = (byte)(v | 0x80);
                intermediate.Add((byte)nextEntry);
                v >>= 7;
            }
            intermediate.Reverse(); // have to reverse this intermediate result to get the right order
            result.AddRange(intermediate);
        }
        return result.ToArray();
    }
    public static uint[] Decode(uint[] bytes)
    {
        if ((bytes[bytes.Length - 1] & 0x80) != 0) throw new InvalidOperationException("Last entry must have a 0 continuation bit");
        var result = new List<uint>();
        int i = 0;
        while (i < bytes.Length)
        {
            int count = 1;
            while ((bytes[i] & 0x80) != 0 ) // count the bytes that make up a number
            {
                bytes[i] -= 0x80;  // could also do another &= 0x7F here, but since I know that the continuation bit is set...
                count++;
                i++;
            }
            i++; // increment to the first byte of the next number
            int shift = 7*(count - 1);
            uint b = 0;
            do
            {
                b += bytes[i - count] << shift;  // go through the bytes from left to right and shift them by the right multiple of 7 bits
                shift -= 7;
                count--;
            } while (count > 0);
            result.Add(b);
        }
        return result.ToArray();
    }
    
}