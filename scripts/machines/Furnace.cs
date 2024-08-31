using Godot;
using System.Collections.Generic;

public partial class Furnace : Machine
{
	public override Dictionary<Resource, int> Inventory { get => base.Inventory; set => base.Inventory = value; }

	public override void OnInteract()
	{
		GD.Print("Furnace clicked");
	}
}