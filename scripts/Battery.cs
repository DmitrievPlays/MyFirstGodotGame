using Godot;

public partial class Battery : Node3D
{
    [Export]
    public Label3D stats;

    private float _batteryLevel;
    private float _maxBatteryLevel;
}
