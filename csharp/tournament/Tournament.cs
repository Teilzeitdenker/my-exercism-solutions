using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public static class Tournament
{
    public static void Tally(Stream inStream, Stream outStream)
    {
        var encoding = new UTF8Encoding();
        byte[] buffer = new byte[inStream.Length];
        inStream.Read(buffer);
        string matches = encoding.GetString(buffer);
        
        string result = ReadAndEvaluateMatches(matches.Split("\n", StringSplitOptions.RemoveEmptyEntries));
   
        outStream.Write(encoding.GetBytes(result));
    }

    private static string ReadAndEvaluateMatches(string[] matches)
    {
        List<Team> teams = new List<Team>();

        List<string> resultString = new List<string>();
        resultString.Add(String.Format("{0,-30} | MP |  W |  D |  L |  P", "Team"));

        foreach (string match in matches)
        {
            if (match == "") return String.Join('\n', resultString.ToArray());
            
            string[] splitted = match.Split(";");
            string firstTeam = splitted[0];
            string secondTeam = splitted[1];
            string resultOfMatch = splitted[2];
            
            int pointsFirstTeam = Points(resultOfMatch, false);
            int indexFirstTeam = teams.FindIndex(t => t.Name == firstTeam);
            
            int pointsSecondTeam = Points(resultOfMatch, true);
            int indexSecondTeam = teams.FindIndex(t => t.Name == secondTeam);
            
            if (indexFirstTeam >= 0) teams[indexFirstTeam].Results.Add(pointsFirstTeam);
            else teams.Add(new Team(firstTeam, pointsFirstTeam));
            if (indexSecondTeam >= 0) teams[indexSecondTeam].Results.Add(pointsSecondTeam);
            else teams.Add(new Team(secondTeam, pointsSecondTeam));
        }
        teams.Sort(new NameComparison());
        teams.Sort(new PointComparison());
        foreach (Team team in teams)
        {
            resultString.Add(team.ToString());
        }
        return String.Join('\n', resultString.ToArray());
    }

    private struct Team
    {
        public string Name;
        public List<int> Results;
        public Team(string name, int result)
        {
            Name = name;
            Results = new List<int> { result };
        }
        public override string ToString()
        {
            int matchesPlayed = Results.Count();
            int won = Results.Where(n => n == 3).Count();
            int draws = Results.Where(n => n == 1).Count();
            int lost = Results.Where(n => n == 0).Count();
            int points = Results.Sum();
            return String.Format("{0, -30} | {1, 2} | {2, 2} | {3, 2} | {4, 2} | {5, 2}", Name, matchesPlayed, won, draws, lost, points);
        }
    }

    private class NameComparison : IComparer<Team>
    {
        int IComparer<Team>.Compare(Team x, Team y)
        {
            return String.Compare(x.Name, y.Name);
        }
    }
    private class PointComparison : IComparer<Team>
    {
        int IComparer<Team>.Compare(Team x, Team y)
        {
            return y.Results.Sum().CompareTo(x.Results.Sum());
        }
    }

    private static int Points(string result, bool inverse)
    {
        if (inverse == false)
        return result switch
        {
            "win" => 3,
            "draw" => 1,
            "loss" => 0,
            _ => throw new ArgumentOutOfRangeException("Result of match is not valid!")
        };
        else return result switch
        {
            "win" => 0,
            "draw" => 1,
            "loss" => 3,
            _ => throw new ArgumentOutOfRangeException("Result of match is not valid!")
        };
    }
}

