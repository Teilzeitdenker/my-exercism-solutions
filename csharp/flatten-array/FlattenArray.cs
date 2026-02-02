using System;
using System.Collections;
using System.Linq;

public static class FlattenArray
{
    public static IEnumerable Flatten(IEnumerable input)
    {
        foreach (object element in input)
        {
            if (element is IEnumerable)
            {
                IEnumerable innerObjects = Flatten(element as IEnumerable);
                foreach (object innerObject in innerObjects)  yield return innerObject; 
            } else
            {
                if (element != null)  yield return element; 
            }
        }
    }
}