using System;

public class CalculationException : Exception
{
    public CalculationException(int operand1, int operand2, string message, Exception inner)
        :base(message, inner)
    {
        Operand1 = operand1;
        Operand2 = operand2;
    }

    public int Operand1 { get; }
    public int Operand2 { get; }
}

public class CalculatorTestHarness
{
    private Calculator calculator;

    public CalculatorTestHarness(Calculator calculator)
    {
        this.calculator = calculator;
    }

    public string TestMultiplication(int x, int y)
    {
        try
        {
            this.Multiply(x, y);
            return "Multiply succeeded";
        }
        catch (CalculationException e) when (x < 0 && y < 0)
        {
            return "Multiply failed for negative operands. " + e.InnerException.Message;
        }
        catch (CalculationException e)
        {
            return "Multiply failed for mixed or positive operands. " + e.InnerException.Message;
        }

    }

    public void Multiply(int x, int y)
    {
        try
        {
            this.calculator.Multiply(x, y);
        }
        catch (OverflowException e)
        {
            throw new CalculationException(x, y, "Exception when calling Multiply", e);
        }
    }
}


// Please do not modify the code below.
// If there is an overflow in the multiplication operation
// then a System.OverflowException is thrown.
public class Calculator
{
    public int Multiply(int x, int y)
    {
        checked
        {
            return x * y;
        }
    }
}
