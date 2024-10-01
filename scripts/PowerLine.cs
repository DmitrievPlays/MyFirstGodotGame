using Godot;

public class PowerLine
{
    private List<PowerNode> _nodes;

    public PowerLine(PowerNode node)
    {
        _nodes.Add(node);
    }
}
