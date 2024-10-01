using Godot;

public partial class Streetlight : Node3D, IElectricModule
{
    [Export]
    public SpotLight3D light;

    [Export]
    public PowerNode powerNode;

    public IEnumerable<IElectricModule> ConnectedModules { get; set; }
    public decimal PowerDemand { get; set; }
    public decimal PowerOutput { get; set; }
}
