using Godot;

public partial class CraftingTable : Machine
{
    [Export]
    public CraftingTableScreen CraftingTableScreen;

    public override void OnInteract()
    {
        CraftingTableScreen.Show();
    }
}
