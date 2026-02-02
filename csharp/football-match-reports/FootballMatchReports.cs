using System;

public static class PlayAnalyzer
{
    public static string AnalyzeOnField(int shirtNum)
    {
        return shirtNum switch
        {
            1 => "goalie",
            2 => "left back",
            3 => "center back",
            4 => "center back",
            5 => "right back",
            6 => "midfielder",
            7 => "midfielder",
            8 => "midfielder",
            9 => "left wing",
            10 => "striker",
            11 => "right wing",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static string AnalyzeOffField(object report)
    {
        switch (report)
        {
            case int i:
                return $"There are {i} supporters at the match.";
            case string s:
                return s;
            case Incident r:
                if (r.GetType().Name == "Injury") {
                    return "Oh no! " + r.GetDescription() + " Medics are on the field.";
                } else return r.GetDescription();
            case Manager m:
                if (m.Club != null) return $"{m.Name} ({m.Club})";
                else return $"{m.Name}";
            default:
                throw new ArgumentException();
        }
    }
}
