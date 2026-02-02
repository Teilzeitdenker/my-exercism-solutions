using System;
using System.Collections.Generic;
using System.Linq;

public class BowlingGame
{
    private List<int> _rolls = new List<int>();
    
    public bool RollsAreValid(IEnumerable<int> rls, int round) 
    {
        if (!rls.Any() || rls.Count() == 1) return true; // well, not complete, but ok
        var ls = rls.ToList();
        switch (ls, round) {
            // cases for the tenth round
            case ([_, _, _, _, ..], 10): // maximum is three rolls for the last round
                return false;
            case ([10, 10, _], 10): // that's another strike
                return true;
            case ([10, var fst, var snd], 10) when fst + snd > 10: // too much for a spare, not allowed
                return false;
            case ([10, ..], 10): // everything else is ok for a strike in the last round (rolls may only be incomplete)
                return true;
            case ([var fst, var snd, ..], 10) when fst + snd > 10: // too much for a spare
                return false;
            case ([var fst, var snd, _], 10) when fst + snd == 10: // ok to roll another one if it's a spare
                return true;
            case ([_, _], 10): // maybe incomplete (when spare) but ok
                return true;
            // cases before the tenth round
            case ([10, ..], < 10): // strike, so skip this
                return RollsAreValid(rls.Skip(1), round + 1);
            case ([var fst, var snd, ..], _) when fst + snd > 10: // too much for a spare
                return false;
            case ([_, _, ..], < 10): // round is complete, skip it
                return RollsAreValid(rls.Skip(2), round + 1);
            // can't think of any more cases, so set the default to false
            default:
                return false;
        }
    }
    public void Roll(int pins) 
    {
        if (pins < 0 || pins > 10) throw new ArgumentException(); // keep our _rolls clean
        _rolls.Add(pins);
        if (!RollsAreValid(_rolls, 1)) throw new ArgumentException(); // check validity before continuing
    }
    
    public int recScore(IEnumerable<int> rls, int acc, int round)
    {
        // keep in mind: the validity was checked by RollsAreValid,
        // but now I have to catch the cases where the list of rolls is incomplete!
        if (!rls.Any() || rls.Count() == 1)
            throw new ArgumentException(); 
        var ls = rls.ToList();
        switch (ls, round)
        {
            // cases for the tenth round
            case ([10, _], 10):
                throw new ArgumentException(); // one last roll is missing
            case ([10, var fst, var snd], 10):
                return acc + 10 + fst + snd;
            case ([_, _, var fst], 10): // first two must be a spare then
                return acc + 10 + fst;
            case ([var fst, var snd], 10) when fst + snd == 10: // one last roll is missing
                throw new ArgumentException();
            case ([var fst, var snd], 10):
                return acc + fst + snd;
            // cases before the tenth round
            case ([10, var fst, var snd, ..], < 10):
                return recScore(rls.Skip(1), acc + 10 + fst + snd, round + 1);
            case ([var fst, var snd, var trd, ..], < 10) when fst + snd == 10:
                return recScore(rls.Skip(2), acc + 10 + trd, round + 1);
            case ([var fst, var snd, ..], < 10):
                return recScore(rls.Skip(2), acc + fst + snd, round + 1);
            default: // all other cases throw
                throw new ArgumentException();
            
        }
    }
    
    public int? Score()
    {
        return recScore(_rolls, 0, 1);
    }
}