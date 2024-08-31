using Godot;

public abstract partial class Machine : StaticBody3D
{
	public Inventory Inventory = new Inventory();

	public abstract void OnInteract(); 
}
