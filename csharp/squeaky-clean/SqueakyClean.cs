using System;
using System.Text;

public static class Identifier
{
    public static string Clean(string identifier)
    {
        if (identifier == "")
        {
            return "";
        }
        StringBuilder builder = new StringBuilder();
        bool found_a_hyphen = false;
        foreach (char c in identifier)
        {
            if (Char.IsWhiteSpace(c))
            {
                builder.Append('_');
                continue;
            }
            else if (Char.IsControl(c))
            {
                builder.Append("CTRL");
            }
            else if (c == '-')
            {
                found_a_hyphen = true;
            }
            else if (Char.IsLetter(c))
            {
                if (c < 'α' || c > 'ω')
                {
                    if (found_a_hyphen)
                    {
                        builder.Append(Char.ToUpper(c));
                        found_a_hyphen = false;
                    } 
                    else
                    {
                        builder.Append(c);
                    }
                }
            }
            
        }
        return builder.ToString();
    }
}
