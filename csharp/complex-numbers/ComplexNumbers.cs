using System;

public struct ComplexNumber
{
    private double _real;
    private double _imaginary;
    public ComplexNumber(double real, double imaginary) => (_real, _imaginary) = (real, imaginary);
    public double Real() => _real;
    public double Imaginary() => _imaginary;
    public ComplexNumber Mul(ComplexNumber other) => new ComplexNumber(_real * other.Real() - _imaginary * other.Imaginary(), _real * other.Imaginary() + _imaginary * other.Real());
    public ComplexNumber Add(ComplexNumber other) => new ComplexNumber(_real + other.Real(), _imaginary + other.Imaginary());
    public ComplexNumber Sub(ComplexNumber other) => new ComplexNumber(_real - other.Real(), _imaginary - other.Imaginary());
    public ComplexNumber Div(ComplexNumber other)
    {
        if (other.Abs() == 0.0) throw new ArgumentException("Division by zero is forbidden!");
        return new ComplexNumber((_real * other.Real() + _imaginary * other.Imaginary()) / Math.Pow(other.Abs(), 2), (- _real * other.Imaginary() + _imaginary * other.Real()) / Math.Pow(other.Abs(), 2));
    }
    public double Abs() => Math.Sqrt(Math.Pow(_real, 2) + Math.Pow(_imaginary, 2));
    public ComplexNumber Conjugate() => new ComplexNumber(_real, -_imaginary);
    public ComplexNumber Exp() => new ComplexNumber(Math.Exp(_real) * Math.Cos(_imaginary), Math.Exp(_real) * Math.Sin(_imaginary));
}