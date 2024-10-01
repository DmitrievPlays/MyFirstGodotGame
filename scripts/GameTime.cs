using Godot;

public partial class GameTime : ResourcePreloader
{
    public double _timeSinceStart; // Milliseconds

    [Export]
    public int _day_length = 20; // Minutes

    public static GameTime Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _Process(double delta)
    {
        _timeSinceStart += delta * 1000;
    }

    public float getDaysElapsed()
    {
        return (float)(_timeSinceStart / (_day_length * 60 * 1000));
    }

    public float getDayNightCycleTimePercentage()
    {
        return (float)((_timeSinceStart % (_day_length * 60 * 1000)) / (_day_length * 60 * 1000)); // Day elapsed time in percent
    }

    public float getCurrentDayTime()
    {
        return (float)((_timeSinceStart % (_day_length * 60 * 1000)) * (((24 * 60 * 60 * 1000) / (_day_length * 60 * 1000)))); // 900000 Milliseconds = 15 minutes
    }
}
