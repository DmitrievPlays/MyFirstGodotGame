using Godot;

public partial class Flashlight : BaseItem
{
    [Export]
    public int Brightness = 1;

    public override void Interact(PlayerControl player)
    {
        base.Interact(player);

        // Default interaction behavior (can be empty or provide a base implementation)
        GD.Print($"And turned on {Name}.");
    }
}
