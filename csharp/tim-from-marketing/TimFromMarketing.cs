using System;

static class Badge
{
    public static string Print(int? id, string name, string? department)
    {
        string id_part = default;
        string dep_part = default;
        if (id != null)
        {
            id_part = $"[{id?.ToString()}] - ";
        } 
        if (department == null)
        {
            dep_part = " - OWNER";
        } 
        else
        {
            dep_part = $" - {department?.ToUpper()}";
        }
        return id_part + $"{name}" + dep_part;
    }
}
