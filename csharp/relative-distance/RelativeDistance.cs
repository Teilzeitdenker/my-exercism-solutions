public class RelativeDistance
{
    private readonly Dictionary<string, HashSet<string>> _graph;

    public RelativeDistance(Dictionary<string, string[]> familyTree)
    {
        _graph = new();
        foreach (var (parent, children) in familyTree)
            foreach (var child in children)
            {
                _graph.TryAdd(parent, new());  _graph.TryAdd(child, new());
                _graph[parent].Add(child);  _graph[child].Add(parent);
                _graph[child].UnionWith(children.Where(s => s != child));
            }
    }

    public int DegreeOfSeparation(string personA, string personB)
    {
        var visited = new HashSet<string> { personA };
        var queue = new Queue<(string, int)>();
        queue.Enqueue((personA, 0));
        while (queue.TryDequeue(out var entry))
        {
            var (currentPerson, dist) = entry;
            if (currentPerson == personB) return dist;
            if (_graph.TryGetValue(currentPerson, out var neighbors))
                foreach (var neighbor in neighbors)
                    if (visited.Add(neighbor))
                        queue.Enqueue((neighbor, dist + 1));
        }
        return -1; // no connection
    }
}
