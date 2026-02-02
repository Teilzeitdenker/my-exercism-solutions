using System;

public class BankAccount
{
    private bool _isClosed = false;
    private decimal _balance = 0m;
    public void Open()
    {
        return;
    }

    public void Close()
    {
        _isClosed = true;
    }

    public decimal Balance
    {
        get
        {
            if (_isClosed) throw new InvalidOperationException();
            return _balance;
        }
        private set
        {
            _balance = value;
        }
    }

    public void UpdateBalance(decimal change)
    {
        lock(this)
        {
            Balance += change;
        }
        return;
    }
}
