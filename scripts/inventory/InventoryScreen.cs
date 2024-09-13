using Godot;
using System.Collections.Generic;

public abstract partial class InventoryScreen : Control
{
	protected PackedScene InventoryItem { get; set; }

	public override void _Ready()
	{
		InventoryItem = ResourceLoader.Load<PackedScene>("res://InventoryItem.tscn");
	}

	public virtual void ShowScreen(Inventory inventory)
	{
		Visible = true;
	}
}
