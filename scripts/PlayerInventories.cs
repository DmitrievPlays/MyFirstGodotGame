using Godot;
using System.Collections.Generic;

public partial class PlayerInventories : Node
{
	public Dictionary<string, Inventory> Inventories = new();

	public static PlayerInventories Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;

		Inventories.Add("Player01", new PlayerInventory());
		Inventories.Add("Player02", new PlayerInventory());
	}

	public Inventory GetPlayerInventory(string name)
	{
		return Inventories[name];
	}
}
