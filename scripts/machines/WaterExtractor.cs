using Godot;
using System.Collections.Generic;

public partial class WaterExtractor : Machine
{
	public Inventory inventory;

	public override void _Ready()
	{
		inventory = new Inventory(3);
		inventory.AddItem(1, new GoldBar(), 3);
	}

	public override void OnInteract()
	{
		GD.Print("Water extractor clicked, it has " + inventory.GetMaxSlots() + " slots of inventory, and " + inventory.GetFreeSlots() + " free slots");
		Node inventoryUI = GetTree().Root.FindChild("InventoryScreen", true, false);
		
		Node content = inventoryUI.FindChild("GridContainer");
	}
}