using System;

public static class ErrorHandling
{
    public static void HandleErrorByThrowingException()
    {
        throw new Exception();
    }

    public static int? HandleErrorByReturningNullableType(string input)
    {
        try
        {
            return int.Parse(input);
        }
        catch
        {
            return null;
        }
    }

    public static bool HandleErrorWithOutParam(string input, out int result)
    {
        try
        {
            result = int.Parse(input);
            return true;
        }
        catch
        {
            result = -1;
            return false;
        }
    }

    public static void DisposableResourcesAreDisposedWhenExceptionIsThrown(IDisposable disposableObject)
    {
        try
        {
            throw new Exception();
        }
        catch
        {
            disposableObject.Dispose();
            throw;
        }
    }
}
