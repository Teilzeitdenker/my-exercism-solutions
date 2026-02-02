using System;

public static class SimpleCalculator
{
    public static string Calculate(int operand1, int operand2, string operation)
    {
        string[] operations = new[] { "+", "*", "/" };
        string result = string.Format("{0:d} {1} {2:d} = ", operand1, operation, operand2);
       
        if (operation == "")
        {
            throw new ArgumentException();
        } else if (operation == null)
        {
            throw new ArgumentNullException();
        } else if (Array.IndexOf(operations, operation) == -1)
        {
            throw new ArgumentOutOfRangeException();
        } else 
        {
            switch (operation)
            {
                case "+":
                    return result + (operand1 + operand2).ToString();
                case "*":
                    return result + (operand1 * operand2).ToString();
                case "/":
                    if (operand2 == 0)
                    {
                        return "Division by zero is not allowed.";
                    } else
                    {
                        return result + (operand1 / operand2).ToString();
                    }
                default:
                    return "";
            }
        } 
    }
}
