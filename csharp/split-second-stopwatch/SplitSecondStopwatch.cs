public enum StopwatchState
{
    Ready,
    Running,
    Stopped
}

public class SplitSecondStopwatch(TimeProvider time)
{    
    public StopwatchState State { get; private set; } = StopwatchState.Ready;
    public TimeSpan Total { 
        get
        {
            if (_startTime == null) return TimeSpan.Zero;
            return State == StopwatchState.Running 
                ? time.GetUtcNow() - _startTime.Value + _accumulatedTotal
                : _accumulatedTotal;
        }
    }
    public TimeSpan CurrentLap
    {
        get
        {
            if (_lapStartTime == null) return TimeSpan.Zero;
            return State == StopwatchState.Running
                ? time.GetUtcNow() - _lapStartTime.Value + _accumulatedCurrentLap
                : _accumulatedCurrentLap;
        }
    }
    public IReadOnlyCollection<TimeSpan> PreviousLaps => _previousLaps.AsReadOnly();

    private DateTimeOffset? _startTime;
    private DateTimeOffset? _lapStartTime;
    private TimeSpan _accumulatedTotal;
    private TimeSpan _accumulatedCurrentLap;
    private readonly List<TimeSpan> _previousLaps = [];

    public void Start()
    {
        if (State == StopwatchState.Running)
            throw new InvalidOperationException("Start cannot be called from running state.");

        // only call GetUtcNow once per Start/Stop/Lap to ensure consistency between total and lap times
        var now = time.GetUtcNow(); 
        _startTime = now;
        _lapStartTime = now;

        State = StopwatchState.Running;
    }

    public void Stop()
    {
        if (State != StopwatchState.Running)
            throw new InvalidOperationException("Stop can only be called from running state.");

        var now = time.GetUtcNow();
        _accumulatedCurrentLap += now - _lapStartTime!.Value; // null forgiving operator "!"
        _accumulatedTotal += now - _startTime!.Value;

        State = StopwatchState.Stopped;
    }

    public void Reset()
    {
        if (State != StopwatchState.Stopped) 
            throw new InvalidOperationException("Reset can only be called from stopped state.");
        
        _startTime = null;
        _lapStartTime = null;
        _accumulatedTotal = TimeSpan.Zero;
        _accumulatedCurrentLap = TimeSpan.Zero;
        _previousLaps.Clear();

        State = StopwatchState.Ready;

    }

    public void Lap()
    {
        if (State != StopwatchState.Running)        
            throw new InvalidOperationException("Lap can only be called from running state.");

        var now = time.GetUtcNow(); 
        _accumulatedCurrentLap += now - _lapStartTime!.Value;
        _previousLaps.Add(_accumulatedCurrentLap);
        _accumulatedCurrentLap = TimeSpan.Zero;
        _lapStartTime = now;
    }
}
