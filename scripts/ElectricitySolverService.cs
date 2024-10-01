using Godot;

public partial class ElectricitySolverService : ResourcePreloader
{
    public Dictionary<int, PowerLine> _powerLines = [];

    public PowerNode _selectedNode;

    public static ElectricitySolverService Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public void CreatePowerLine(PowerNode node)
    {
        PowerLine line = new(node);
        _powerLines.Add(1, line);
    }

    public void JoinLines(PowerLine line)
    {
    }

    public static void SplitPowerLine()
    {
    }
}
