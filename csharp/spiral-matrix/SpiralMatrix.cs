using System;
using System.Numerics;

public class SpiralMatrix
{
    // inspired by JakDrako on https://www.reddit.com/r/dailyprogrammer/comments/6i60lr/20170619_challenge_320_easy_spiral_ascension/
    public static int[,] GetMatrix(int size)
    {
        int[,] matrix = new int[size, size];
        // Rotate the coordinate system such that the imaginary axis points to the right and the real axis down!
        // Then we start outside the board with the following position and direction
        var position = new Complex(0, -1); 
        var direction = new Complex(0, 1); 
        int numSpiralPartsWithThisSize = 1; // e.g. for n=3 the lengths of straight spiral parts are 3, 2, 2, 1, 1
        int counter = 1;
        while (size > 0)
        {
            for (int i = 0; i < size; i++) // walk one such straight part
            {
                position += direction;
                matrix[(int)position.Real, (int)position.Imaginary] = counter; // Rows (first index) correspond to the real part,
                // column index grows with the imaginary part
                counter++;
            }
            direction *= - Complex.ImaginaryOne; // now rotate 90 degrees to the right ( multiply with  -i )
            if (--numSpiralPartsWithThisSize == 0) // look if there are straight parts left
            {
                numSpiralPartsWithThisSize = 2;
                size--;
            }
        }
        return matrix;
    }
}
