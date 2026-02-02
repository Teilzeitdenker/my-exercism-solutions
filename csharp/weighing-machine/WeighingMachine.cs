using System;
using System.Globalization;
using System.Threading;

class WeighingMachine
{
    public int Precision { get; set; }

    private double _weight;
    public double Weight
    {
        get { return _weight; }
        set { if (value < 0.0) {
                throw new ArgumentOutOfRangeException();
            } else
            {
                _weight = value;
            }
        }
    }
    public string DisplayWeight
    {
        get
        {
            // Setze die CultureInfo im aktuellen Thread auf en-US, damit Kommazahlen mit einem Punkt formatiert werden!
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            double correctedWeight = Weight - TareAdjustment;
            string format = $"f{Precision:d}";
            string formattedDisplay = correctedWeight.ToString(format);
            return $"{formattedDisplay} kg";
        }
    }

    public double TareAdjustment { get; set; } = 5.0;

    public WeighingMachine(int precision)
    {
        Precision = precision;
    }
}
