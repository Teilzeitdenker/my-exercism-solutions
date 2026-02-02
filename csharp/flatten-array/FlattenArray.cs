using System;
using System.Collections;

public static class FlattenArray
{
    public static IEnumerable Flatten(IEnumerable input)
    {
        foreach (object element in input)
        {
            if (element is IEnumerable)
            {
                foreach (object innerObj in Flatten(element as IEnumerable))  yield return innerObj; 
            }
            else if (element != null) yield return element;
        }
    }
}