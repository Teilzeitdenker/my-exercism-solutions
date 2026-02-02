using System;
using System.Collections.Generic;
using System.Linq;

public struct Coord
{
    public Coord(ushort x, ushort y)
    {
        X = x;
        Y = y;
    }

    public ushort X { get; }
    public ushort Y { get; }
    //overload operator "-" to give the distance (squared) between two coordinates
    public static int operator -(Coord c1, Coord c2)
    {
        return (c1.X - c2.X)*(c1.X - c2.X) + (c1.Y - c2.Y)*(c1.Y - c2.Y);
    }
}

public struct Plot
{
    public Plot(Coord c1, Coord c2, Coord c3, Coord c4)
    {
        C1 = c1;
        C2 = c2;
        C3 = c3;
        C4 = c4;
    }
    public Coord C1 { get; }
    public Coord C2 { get; }
    public Coord C3 { get; }
    public Coord C4 { get; }
    public int LongestSide()
    {
        var lengths = new int[] { C1 - C2, C2 - C3, C3 - C4, C4 - C1};
        return lengths.Max();
    }
}


public class ClaimsHandler
{
    private List<Plot> claims = new List<Plot>();
    public void StakeClaim(Plot plot)
    {
        if (!IsClaimStaked(plot)) claims.Add(plot);
    }
    public bool IsClaimStaked(Plot plot) => claims.Contains(plot);
    public bool IsLastClaim(Plot plot) => plot.Equals(claims.Last());
    public Plot GetClaimWithLongestSide() => (claims.OrderBy(o => o.LongestSide()).ToList()).Last();
}
