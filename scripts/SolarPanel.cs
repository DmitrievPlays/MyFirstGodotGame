using Godot;

public partial class SolarPanel : Node3D, IElectricModule
{
    [Export]
    public SpotLight3D light;

    [Export]
    public PowerNode powerNode;

    public IEnumerable<IElectricModule> ConnectedModules { get; set; }
    public decimal PowerDemand { get; set; }
    public decimal PowerOutput { get; set; }

    private bool _isThereSun = false;
    private GameTime _gameTime;

    public override void _Ready()
    {
        _gameTime = GameTime.Instance;
        UpdateStateAsync();
    }

    public async void UpdateStateAsync()
    {
        if (_gameTime.getDayNightCycleTimePercentage() > 0.3 && _gameTime.getDayNightCycleTimePercentage() < 0.85)
            _isThereSun = true;
        else
            _isThereSun = false;

        light.Visible = _isThereSun;

        await Task.Delay(1000);
        UpdateStateAsync();
    }
}
