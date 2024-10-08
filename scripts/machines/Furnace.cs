﻿using Godot;
using System.Collections.Generic;

public partial class Furnace : Machine
{
	public Inventory inventory;

	public override void _Ready()
	{
		inventory = new Inventory(6);
	}

	public override void OnInteract()
	{
		GD.Print("Furnace clicked, it has " + inventory.GetMaxSlots() + " slots of inventory, and " + inventory.GetFreeSlots() + " free slots");
		Node inventoryUI = GetTree().Root.FindChild("InventoryScreen", true, false);
		
		Node content = inventoryUI.FindChild("GridContainer");
	}
}