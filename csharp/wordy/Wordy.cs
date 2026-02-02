using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

public static class Wordy
{
    public static int Answer(string question)
    {
        var parsing = Grammar.Question.TryParse(question);
        if (parsing.WasSuccessful)
        {
            var res = parsing.Value;
            return res.Operations.Aggregate(res.StartNumber, (acc, op) => op.OperateOn(acc));
        }
        else throw new ArgumentException();
    }
}

public static class Grammar
{
    public static readonly Parser<String> StartOfQuestion = Parse.String("What is").Text().Token();
    public static readonly Parser<char> EndOfQuestion = Parse.Char('?').End();
    public static readonly Parser<int> Number = 
        from sign in Parse.Optional(Parse.Char('-')) 
        from num in Parse.Number 
        from ws in Parse.Char(' ').Many()
        select int.Parse(num) * (sign.IsDefined ? -1 : 1);
    public static readonly Parser<OperationType> OpType =
        Parse.String("plus").Token().Return(OperationType.Add)
            .Or(Parse.String("minus").Token().Return(OperationType.Sub))
            .Or(Parse.String("multiplied by").Token().Return(OperationType.Mul))
            .Or(Parse.String("divided by").Token().Return(OperationType.Div));
    public static readonly Parser<Operation> Operation =
        from opType in OpType
        from num in Number
        select new Operation(opType, num);
    public static readonly Parser<Question> Question = 
        from st in StartOfQuestion 
        from num in Number 
        from ops in Operation.Many()
        from en in EndOfQuestion
        select new Question(num, ops);
}

public enum OperationType
{
    Add,
    Sub,
    Mul,
    Div
}

public class Operation
{
    public Operation(OperationType op, int n) => (Op, Number) = (op, n);
    public OperationType Op { get; set; }
    public int Number { get; set; }
    public int OperateOn(int a) => Op switch 
    { 
        OperationType.Add => a + Number,
        OperationType.Sub => a - Number,
        OperationType.Mul => a * Number,
        OperationType.Div => a / Number,
        _ => throw new NotSupportedException()
    };
}

public class Question
{
    public Question(int n, IEnumerable<Operation> ops) => (StartNumber, Operations) = (n, ops);
    public int StartNumber { get; set; }
    public IEnumerable<Operation> Operations { get; set; }
}