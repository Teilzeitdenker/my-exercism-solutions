using System;

public class Orm : IDisposable
{
    private Database database;

    public Orm(Database database)
    {
        this.database = database;
    }

    public void Begin()
    {
        try
        {
            this.database.BeginTransaction();
        }
        catch (InvalidOperationException)
        {
            this.database.Dispose();
        }
    }

    public void Write(string data)
    {
        try
        {
            this.database.Write(data);
        }
        catch (InvalidOperationException)
        {
            this.database.Dispose();
        }
    }

    public void Commit()
    {
        try
        {
            this.database.EndTransaction();
        }
        catch (InvalidOperationException)
        {
            this.database.Dispose();
        }
    }
    public void Dispose()
    {
        this.database.Dispose();
    }
}
