using Godot;

public partial class WaterExtractor : Machine
{
	public WaterExtractorInventory Inventory = new();
	private CustomInventoryScreen screen;

	public override async void _Ready()
	{
		screen = GetNode<CustomInventoryScreen>("/root/MainScene/MainUI/CustomScreen");
		var manager = ResourceManager.Instance;

		Inventory.AddItem(await manager.GetResource(OriginalResources.ResourceType.GOLD), 5);
		Inventory.AddItem(await manager.GetResource(OriginalResources.ResourceType.IRON), 15);
	}

	public override void OnInteract()
	{
		screen.ShowScreen(Inventory, "Water extractor");
	}
}