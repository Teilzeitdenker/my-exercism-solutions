using System;
using System.Numerics;

public class SpiralMatrix
{
    // inspired by JakDrako on https://www.reddit.com/r/dailyprogrammer/comments/6i60lr/20170619_challenge_320_easy_spiral_ascension/
    public static int[,] GetMatrix(int size)
    {
        int[,] matrix = new int[size, size];
        var position = new Complex(-1, 0); // start outside of the "board"
        var direction = new Complex(1, 0); // heading to the right
        int numSpiralPartsWithThisSize = 1; // e.g. for n=3 the lengths of straight spiral parts are 3, 2, 2, 1, 1
        int counter = 1;
        while (size > 0)
        {
            for (int i = 0; i < size; i++) // walk one such straight part
            {
                position += direction;
                matrix[(int)position.Imaginary, (int)position.Real] = counter; // in order to get it right, the ROWS of the matrix (first index)
                // have to vary with the imaginary part, while the COLUMS (second index) correspond to the real part
                counter++;
            }
            direction *= Complex.ImaginaryOne; // now rotate 90 degrees to the right
            if (--numSpiralPartsWithThisSize == 0) // look if there are straight parts left
            {
                numSpiralPartsWithThisSize = 2;
                size--;
            }
        }
        return matrix;
    }
}
