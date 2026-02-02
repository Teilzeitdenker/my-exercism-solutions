using System;
using System.Collections.Generic;

public class FacialFeatures
{
    public string EyeColor { get; }
    public decimal PhiltrumWidth { get; }

    public FacialFeatures(string eyeColor, decimal philtrumWidth)
    {
        EyeColor = eyeColor;
        PhiltrumWidth = philtrumWidth;
    }
    public override bool Equals(object obj)
    {
        if (!(obj is FacialFeatures temp)) return false;
        return temp.EyeColor == this.EyeColor && temp.PhiltrumWidth == this.PhiltrumWidth;
    }
    // use a hash code based on the string returned by a custom ToString() - method
    public override string ToString()
    {
        return $"[Eye Color: {EyeColor}; Philtrum Width: {PhiltrumWidth}]";
    }
    public override int GetHashCode() => ToString().GetHashCode();
}

public class Identity
{
    public string Email { get; }
    public FacialFeatures FacialFeatures { get; }

    public Identity(string email, FacialFeatures facialFeatures)
    {
        Email = email;
        FacialFeatures = facialFeatures;
    }
    public override bool Equals(object obj)
    {
        if (!(obj is Identity temp)) return false;
        return temp.FacialFeatures.Equals(this.FacialFeatures) && temp.Email == this.Email;
    }
    // use a hash code based on the string returned by a custom ToString() - method
    public override string ToString()
    {
        return $"[Facial Features: {FacialFeatures.ToString()}; Email: {Email}]";
    }
    public override int GetHashCode() => ToString().GetHashCode();
}

public class Authenticator
{
    private HashSet<int> hashedIdentities = new HashSet<int>();
    public static bool AreSameFace(FacialFeatures faceA, FacialFeatures faceB)
    {
        return faceA.Equals(faceB);
    }

    public bool IsAdmin(Identity identity)
    {
        FacialFeatures admin_face = new FacialFeatures("green", 0.9m);
        Identity admin = new Identity("admin@exerc.ism", admin_face);
        return identity.Equals(admin);
    }

    public bool Register(Identity identity)
    {
        return hashedIdentities.Add(identity.GetHashCode());
    }

    public bool IsRegistered(Identity identity)
    {
        if (hashedIdentities.Count == 0) return false;
        return hashedIdentities.Contains(identity.GetHashCode());
    }

    public static bool AreSameObject(Identity identityA, Identity identityB)
    {
        return object.ReferenceEquals(identityA, identityB);
    }
}
