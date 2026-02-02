using System;
public static class TelemetryBuffer
{
    public static byte[] ToBuffer(long reading)
    {
        byte[] bytes;
        var buffer = new byte[9];

        if (reading >= 0)
        {
            if (reading <= ushort.MaxValue)
            {
                bytes = BitConverter.GetBytes((ushort)reading);
                buffer[0] = (byte)bytes.Length;
            }
            else if (reading <= int.MaxValue)
            {
                bytes = BitConverter.GetBytes((int)reading);
                buffer[0] = (byte)(256 - bytes.Length);
            }
            else if (reading <= uint.MaxValue)
            {
                bytes = BitConverter.GetBytes((uint)reading);
                buffer[0] = (byte)bytes.Length;
            }
            else
            {
                bytes = BitConverter.GetBytes(reading);
                buffer[0] = (byte)(256 - bytes.Length);
            }
        }
        else
        {
            if (reading >= short.MinValue)
            {
                bytes = BitConverter.GetBytes((short)reading);
            }
            else if (reading >= int.MinValue)
            {
                bytes = BitConverter.GetBytes((int)reading);
            }
            else
            {
                bytes = BitConverter.GetBytes(reading);
            }
            buffer[0] = (byte)(256 - bytes.Length);
        }
        for (var i = 0; i < bytes.Length; i++)
        {
            buffer[i + 1] = bytes[i];
        }
        return buffer;
    }

    public static long FromBuffer(byte[] buffer)
    {
        var shouldReturnU = buffer[0] <= 8;
        var zeroDifference = shouldReturnU ? buffer[0] : 256 - buffer[0];

        if (buffer[0] > 8 && buffer[0] < 0xf8)
        {
            return 0;
        }

        if (zeroDifference <= 0) return 0;

        if (zeroDifference == 8)
        {
            return BitConverter.ToInt64(buffer, 1);
        }

        if (zeroDifference == 4)
        {
            return shouldReturnU ? (long)BitConverter.ToUInt32(buffer, 1) : BitConverter.ToInt32(buffer, 1);
        }
        else return shouldReturnU ? (int)BitConverter.ToUInt16(buffer, 1) : BitConverter.ToInt16(buffer, 1);
    }
}