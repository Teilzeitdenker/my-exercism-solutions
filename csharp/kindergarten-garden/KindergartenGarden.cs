using System;
using System.Collections.Generic;

public enum Plant
{
    Violets,
    Radishes,
    Clover,
    Grass
}

public class KindergartenGarden
{
    private string _firstRow;
    private string _secondRow;
    public KindergartenGarden(string diagram)
    {
        string[] rows = diagram.Split("\n");
        _firstRow = rows[0];
        _secondRow = rows[1];
    }

    public IEnumerable<Plant> Plants(string student)
    {
        int idx = (int)student[0] - 65;
        yield return charToPlant(_firstRow[2 * idx]);
        yield return charToPlant(_firstRow[2 * idx + 1]);
        yield return charToPlant(_secondRow[2 * idx]);
        yield return charToPlant(_secondRow[2 * idx + 1]);
    }

    private Plant charToPlant(char c)
    {
        return c switch
        {
            'V' => Plant.Violets,
            'R' => Plant.Radishes,
            'C' => Plant.Clover,
            'G' => Plant.Grass,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}