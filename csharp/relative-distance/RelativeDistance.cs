public class RelativeDistance
{
    private readonly Dictionary<string, HashSet<string>> _graph;

    public RelativeDistance(Dictionary<string, string[]> familyTree)
    {
        _graph = new Dictionary<string, HashSet<string>>();
        foreach (var (parent, children) in familyTree)
        {
            foreach (var child in children)
            {
                AddBidirectionalEdge(parent, child);
                foreach (var sibling in children)
                {
                    if (sibling != child)
                    {
                        AddBidirectionalEdge(child, sibling);
                    }
                }
            }
        }
    }

    private void AddBidirectionalEdge(string p1, string p2)           
    {
        if (!_graph.ContainsKey(p1))
        {
            _graph[p1] = new HashSet<string>();
        }
        if (!_graph.ContainsKey(p2))
        {
            _graph[p2] = new HashSet<string>();
        }
        _graph[p1].Add(p2);
        _graph[p2].Add(p1);
    }

    public int DegreeOfSeparation(string personA, string personB)
    {
        if (!_graph.ContainsKey(personA) || !_graph.ContainsKey(personB))
        {
            return -1;
        }

        var visited = new HashSet<string>();
        var queue = new Queue<(string person, int dist)>();
        queue.Enqueue((personA, 0));
        visited.Add(personA);

        while (queue.Count > 0)
        {
            var (currentPerson, dist) = queue.Dequeue();
            if (currentPerson == personB)
            {
                return dist;
            }
            if (_graph.TryGetValue(currentPerson, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (visited.Add(neighbor))
                    {
                        queue.Enqueue((neighbor, dist + 1));
                    }
                }
            }
        }
        return -1; // no connection found
    }
}
