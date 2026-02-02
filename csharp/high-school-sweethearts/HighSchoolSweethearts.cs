using System;
using System.Globalization;


public static class HighSchoolSweethearts
{
    public static string DisplaySingleLine(string studentA, string studentB)
    {
        return $"{studentA,29} ♡ {studentB,-29}";
    }

    public static string DisplayBanner(string studentA, string studentB)
    {
        string[] fNlNA = studentA.Split(" ");
        string[] fNlNB = studentB.Split(" ");
        string fNA = fNlNA[0].Substring(0, 1);
        string lNA = fNlNA[1].Substring(0, 1);
        string fNB = fNlNB[0].Substring(0, 1);
        string lNB = fNlNB[1].Substring(0, 1);
        string heart = 
 $@"
     ******       ******
   **      **   **      **
 **         ** **         **
**            *            **
**                         **
**     { fNA}. { lNA}.  +  { fNB}. { lNB}.     **
 **                       **
   **                   **
     **               **
       **           **
         **       **
           **   **
             ***
              *
";

       
        return heart;
    }

    public static string DisplayGermanExchangeStudents(string studentA
        , string studentB, DateTime start, float hours)
    {
        CultureInfo germany = CultureInfo.GetCultureInfo("de-DE");
        string message = String.Format(germany, "{0} and {1} have been dating since {2:d} - that's {3:n2} hours", studentA, studentB, start, hours);
        return message;
    }
}
