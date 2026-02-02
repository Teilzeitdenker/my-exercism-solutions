using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

public class RestApi
{
    private class ODict : SortedDictionary<string, decimal> {}
    private record UserObject(string name, ODict owes, ODict owed_by)
    { public decimal balance => owed_by.Values.Sum() - owes.Values.Sum(); }
    private record User(string user);
    private record UserList(string[] users);
    private record IOU(string lender, string borrower, decimal amount);
    private Dictionary<string, UserObject> db { get; set; } = new();
    public RestApi(string database)
    {
        var document = JsonSerializer.Deserialize<UserObject[]>(database);
        foreach (var userObj in document) db[userObj.name] = userObj;
    }
    public string Get(string url, string payload = null)
    {
        if (payload == null) return JsonSerializer.Serialize(Array.Empty<string>());
        var wanted = JsonSerializer.Deserialize<UserList>(payload);
        var res = wanted.users.Select(name => db[name]);
        return JsonSerializer.Serialize(res);
    }
    public string Post(string url, string payload) => url switch
    {
         "/add" => AddNewUser(JsonSerializer.Deserialize<User>(payload).user),
        _       => AddIOUEntry(JsonSerializer.Deserialize<IOU>(payload))
    };
    private string AddNewUser(string name)
    {
        db[name] = new(name, new(), new());
        return JsonSerializer.Serialize(db[name]);
    }
    private string AddIOUEntry(IOU iou)
    {
        if (db[iou.lender].owes.ContainsKey(iou.borrower))
        {
            var restDebt = db[iou.lender].owes[iou.borrower] - iou.amount;
            if (restDebt > 0.0m) OnlyUpdateEntries(iou.lender, iou.borrower, restDebt);
            else RemoveAndPossiblyAddEntries(iou.lender, iou.borrower, -restDebt);
        } else AddEntries(iou.lender, iou.borrower, iou.amount);
        var response = new[] { db[iou.lender], db[iou.borrower] }.OrderBy(u => u.name);
        return JsonSerializer.Serialize(response);
    }
    private void OnlyUpdateEntries(string lender, string borrower, decimal amount) =>
        (db[lender].owes[borrower], db[borrower].owed_by[lender]) = (amount, amount);
    private void RemoveAndPossiblyAddEntries(string lender, string borrower, decimal amount)
    {
        db[lender].owes.Remove(borrower);
        db[borrower].owed_by.Remove(lender);
        if (amount > 0.0m) AddEntries(lender, borrower, amount);
    }
    private void AddEntries(string lender, string borrower, decimal amount) =>
        (db[lender].owed_by[borrower], db[borrower].owes[lender]) = (amount, amount);
}