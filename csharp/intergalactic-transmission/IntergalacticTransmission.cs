using System.Numerics;

public static class IntergalacticTransmission
{
    public static byte[] GetTransmitSequence(byte[] message)
    {
        int transmissionLength = (message.Length * 8 + 6) / 7; // ceiling division
        byte[] result = new byte[transmissionLength];
        // Copy 7 data bits at a time into positions 7 to 1 of each transmission byte
        // then add parity bit at position 0 
        for (int i = 0; i < transmissionLength; i++)
        {
            CopyBits(message, i * 7, result, i * 8, 7);
            result[i] |= (byte)((BitOperations.PopCount(result[i]) % 2 == 0) ? 0 : 1);
        }
        return result;
    }

    public static byte[] DecodeSequence(byte[] receivedSeq)
    {
        // check errors in transmission by verifying parity bits
        if (receivedSeq.Any(b => BitOperations.PopCount(b) % 2 != 0)) 
            throw new ArgumentException("Transmission failure. Parity check failed!");

        int decodedLength = (receivedSeq.Length * 7) / 8;
        byte[] result = new byte[decodedLength];
        // Copy 7 data bits at a time from received byte array, just skip parity bits
        for (int i = 0; i < receivedSeq.Length; i++)
        {
            CopyBits(receivedSeq, i * 8, result, i * 7, 7);
        }
        return result;
    }

    // universal helper to copy a certain amount of bits from source to target byte arrays
    // with arbitrary index offsets while ensuring in-bounds access
    private static void CopyBits(byte[] source, int sourceBitIndex, 
                                 byte[] target, int targetBitIndex, 
                                 int bitCount)
    {
        for (int i = 0; i < bitCount; i++)
        {
            int srcAbsoluteIndex = sourceBitIndex + i;
            int srcByteIndex = srcAbsoluteIndex / 8;
            if (srcByteIndex >= source.Length) break;
            int srcBitPosition = 7 - (srcAbsoluteIndex % 8);

            int tgtAbsoluteIndex = targetBitIndex + i;
            int tgtByteIndex = tgtAbsoluteIndex / 8;
            if (tgtByteIndex >= target.Length) break;
            int tgtBitPosition = 7 - (tgtAbsoluteIndex % 8);

            if ((source[srcByteIndex] & (1 << srcBitPosition)) != 0)
            {
                target[tgtByteIndex] |= (byte)(1 << tgtBitPosition);
            }
        }
    }
}
