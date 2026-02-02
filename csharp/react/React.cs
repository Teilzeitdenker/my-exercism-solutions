using System;
using System.Collections.Generic;
using System.Linq;

public class Reactor
{
    public InputCell CreateInputCell(int value) => new InputCell(value);
    public ComputeCell CreateComputeCell(IEnumerable<Cell> producers, Func<int[], int> compute)
        => new ComputeCell(producers, compute);
}

public abstract class Cell
{
    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                Changed?.Invoke(this, _value);
            }
        }
    }
    public event EventHandler<int> Changed; 
}

public class InputCell : Cell
{
    public InputCell(int value) => Value = value;
}

public class ComputeCell : Cell
{
    private IEnumerable<Cell> Producers { get; init; }
    private Func<int[], int> Compute { get; init; }
    public ComputeCell(IEnumerable<Cell> producers, Func<int[], int> compute)
    {
        (Producers, Compute) = (producers, compute);
        ComputeNew();
        foreach (var producer in OnlyInputCells())
            producer.Changed += (o,e) => ComputeNew(); // object and new integer aren't used by the callback
    }
    private void ComputeNew() =>  Value = Compute(Producers.Select(x => x.Value).ToArray());
    private IEnumerable<Cell> OnlyInputCells() 
        => Producers.SelectMany(x => x is ComputeCell c ? c.OnlyInputCells() : [(InputCell)x]).Distinct();
}