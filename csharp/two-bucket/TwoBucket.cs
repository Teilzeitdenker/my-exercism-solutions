using System;
using System.Collections.Generic;
using System.Linq;

public enum Bucket { One, Two }

public class TwoBucketResult
{
    public TwoBucketResult(int moves, Bucket goalBucket, int otherBucket)
    {
        Moves = moves;
        GoalBucket = goalBucket;
        OtherBucket = otherBucket;
    }

    public int Moves { get; init; }
    public Bucket GoalBucket { get; init; }
    public int OtherBucket { get; init; }
}

public class TwoBucket
{
    private record struct State(int Fst, int Snd); // need value equality
    public TwoBucket(int bucketOne, int bucketTwo, Bucket startBucket)
    {
        sizeOne = bucketOne;
        sizeTwo = bucketTwo;
        if (startBucket == Bucket.One)
        {
            stateQueue.Enqueue((new State(sizeOne, 0), 1)); // last entry gives number of moves
            forbiddenStates.Add(new State(0, sizeTwo));
        }
        else
        {
            stateQueue.Enqueue((new State(0, sizeTwo), 1));
            forbiddenStates.Add(new State(sizeOne, 0));
        }
    }

    public TwoBucketResult Measure(int goal)
    {   // handle impossible cases
        if (goal > Math.Max(sizeOne, sizeTwo) || goal % GCD(sizeOne, sizeTwo) != 0) 
            throw new ArgumentException("No solution");
        return Search(goal);
    }

    private TwoBucketResult Search(int goal)
    {
        var (state, moves) = stateQueue.Dequeue();
        var (a, b) = state;
        if (a == goal || b == goal) 
            return new TwoBucketResult(moves, (a == goal) ? Bucket.One : Bucket.Two, (a == goal) ? b : a);
        var nextStates = GetNextStates(state).Except(forbiddenStates).ToList();
        foreach (var newState in nextStates) // update hash set and queue
            forbiddenStates.Add(newState);
        foreach (var newState in nextStates) 
            stateQueue.Enqueue((newState, moves + 1)); // one move more needed to get to this state
        return Search(goal); // recurse
    }
    private IEnumerable<State> GetNextStates(State state)
    {
        var (a, b) = state;
        var pourLeft = Math.Min(sizeOne - a, b);
        var pourRight = Math.Min(sizeTwo - b, a);
        return new List<State>
        {
            new State(sizeOne, b), 
            new State(a, sizeTwo),                             
            new State(0, b), 
            new State(a, 0),                                             
            new State(a + pourLeft, b - pourLeft), 
            new State(a - pourRight, b + pourRight) 
        }.Distinct();
    }

    private static int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);

    private int sizeOne;
    private int sizeTwo;
    private HashSet<State> forbiddenStates = new HashSet<State>();
    private Queue<(State, int)> stateQueue = new Queue<(State, int)>();
}
