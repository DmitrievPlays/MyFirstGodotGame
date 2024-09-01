using CityBuilder.scripts.interfaces;
using Godot;

public abstract partial class Machine : StaticBody3D
{
	public Inventory Inventory;

	public abstract void OnInteract();
}
