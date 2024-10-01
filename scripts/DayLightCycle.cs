using Godot;

public partial class DayLightCycle : DirectionalLight3D
{
    [Export]
    public DirectionalLight3D _sun;

    [Export]
    public WorldEnvironment Environment;

    private GameTime _gameTime;

    private bool smoothingStarted = false;
    private bool methodLock = false;

    public override void _Ready()
    {
        _gameTime = GameTime.Instance;
    }

    public override void _Process(double delta)
    {
        RotationDegrees = new Vector3(_gameTime.getDayNightCycleTimePercentage() * 360 + 65, 0, 0);

        if (_gameTime.getDayNightCycleTimePercentage() * 360 > 105 && _gameTime.getDayNightCycleTimePercentage() * 360 < 106)
            UpEnergyMultiplier();
        else if (_gameTime.getDayNightCycleTimePercentage() * 360 > 270 && _gameTime.getDayNightCycleTimePercentage() * 360 < 271)
            DownEnergyMultiplier();
    }

    private async void UpEnergyMultiplier()
    {
        if (!smoothingStarted)
            smoothingStarted = true;
        else return;

        while (Environment.Environment.BackgroundEnergyMultiplier < 8)
        {
            Environment.Environment.BackgroundEnergyMultiplier += 0.05f;
            await Task.Delay(64);
        }

        smoothingStarted = false;
    }

    private async void DownEnergyMultiplier()
    {
        if (!smoothingStarted)
            smoothingStarted = true;
        else return;

        while (Environment.Environment.BackgroundEnergyMultiplier > 1)
        {
            Environment.Environment.BackgroundEnergyMultiplier -= 0.05f;
            await Task.Delay(64);
        }

        smoothingStarted = false;
    }
}
