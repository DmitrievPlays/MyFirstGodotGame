using Godot;

public partial class PowerNode : MeshInstance3D, IElectricModule
{
    public ElectricitySolverService _solverService;

    //public List<PowerNode> _connectedNodes = [];

    public IEnumerable<IElectricModule> ConnectedModules { get; set; }
    public decimal PowerDemand { get; set; }
    public decimal PowerOutput { get; set; }

    public override void _Ready()
    {
        _solverService = ElectricitySolverService.Instance;
        _solverService.CreatePowerLine(this);
    }

    public void Interact()
    {
        if (_solverService._selectedNode == null)
            return;

        //if (_connectedNodes.Any())
        //{
        //    //_solverService.JoinLines()
        //}
        //else if (_solverService._selectedNode._connectedNodes.Any())
        //{
        //}
    }
}
