using Godot;

public partial class Clock : Node3D
{
    [Export]
    public Label3D time;

    public override void _Process(double delta)
    {
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(GameTime.Instance.getCurrentDayTime());
        string convertedTime = string.Format("{0:D2}:{1:D2}",
                             (int)timeSpan.TotalHours,
                             timeSpan.Minutes);

        time.Text = convertedTime + "\n" + @$"Day {Mathf.Ceil(GameTime.Instance.getDaysElapsed())}";
    }
}
