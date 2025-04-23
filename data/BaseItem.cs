using Godot;
using static OriginalResources;

public partial class BaseItem : Resource
{
    public int Id;

    [Export]
    public string Name;

    [Export]
    public string Description;

    [Export]
    public int MaxPerStack;

    [Export]
    public string Icon;

    [Export]
    public ResourceType Type;

    public virtual void Interact(PlayerControl player)
    {
        // Default interaction behavior (can be empty or provide a base implementation)
        GD.Print($"Interacted with {Name}.");
    }
}
