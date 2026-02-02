using System;
using System.Collections.Generic;
using System.Linq;

public enum Operator
{
    Add, Substract, Multiply, Divide
}
public static class Forth
{
    public static string Evaluate(string[] instructions)
    {
        var list = new List<int>();
        IDictionary<string, string> userInputs = new Dictionary<string, string>();
        foreach (var instruction in instructions)
        {
            if (instruction.Contains(':'))
                userInputs = UserDefinedWords(instruction.ToLowerInvariant(), userInputs);
            else
            {
                string[] split = ReplaceInstuction(userInputs, instruction.ToLowerInvariant()).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in split)
                {
                    list = item switch
                    {
                        "+" => ArithmeticOperatio(list, Operator.Add),
                        "-" => ArithmeticOperatio(list, Operator.Substract),
                        "*" => ArithmeticOperatio(list, Operator.Multiply),
                        "/" => ArithmeticOperatio(list, Operator.Divide),
                        "dup" => list.Any() ? new List<int>(list) { list.Last() } : throw new InvalidOperationException(),
                        "drop" => list.Any() ? list.SkipLast(1).ToList() : throw new InvalidOperationException(),
                        "swap" => Swap(list).ToList(),
                        "over" => Over(list).ToList(),
                        _ when int.TryParse(item, out int value) => new List<int>(list) { value },
                        _ => throw new InvalidOperationException()
                    };
                }
            }
        }

        return string.Join(' ', list);
    }

    private static string ReplaceInstuction(IDictionary<string, string> inputs, string instruction)
    {
        foreach (var (key, value) in inputs)
            instruction = instruction.Replace(key, value);
        return instruction;
    }

    private static IDictionary<string, string> UserDefinedWords(string instruction, IDictionary<string, string> commands)
    {
        var split = instruction.Split(new char[] { ':', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string key = string.Empty, value = string.Empty;
        for (int i = 0; i < split.Length; i++)
        {

            if (i == 0)
                key = int.TryParse(split[i], out int result) ? throw new InvalidOperationException() : split[i];
            else
                value += commands.ContainsKey(split[i]) ? $"{commands[split[i]]} " : $"{split[i]} ";
        }
        if (commands.TryAdd(key, value))
            return commands;
        commands[key] = value;
        return commands;
    }

    private static IEnumerable<int> Swap(IEnumerable<int> numbers) => (numbers.Count() < 2) ? throw new InvalidOperationException() : numbers.Take(numbers.Count() - 2).Concat(numbers.TakeLast(2).Reverse());

    private static IEnumerable<int> Over(IEnumerable<int> numbers) => (numbers.Count() < 2) ? throw new InvalidOperationException() : numbers.Append(numbers.ElementAt(numbers.Count() - 2));

    private static List<int> ArithmeticOperatio(IEnumerable<int> numbers, Operator op)
    {
        if (numbers.Count() < 2)
            throw new InvalidOperationException();
        var result = numbers.Aggregate((acc, cur) => {
            return op switch
            {
                Operator.Add => acc + cur,
                Operator.Substract => acc - cur,
                Operator.Multiply => acc * cur,
                Operator.Divide => acc / cur,
                _ => throw new InvalidOperationException()
            };
        });

        return new[] { result }.ToList();
    }

}