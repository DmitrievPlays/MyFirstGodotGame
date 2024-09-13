using Godot;

public partial class Pickaxe : Resource
{
	public Pickaxe()
	{
		Name = "Pickaxe";
		Description = "You can mine some resources with that thing";
		MaxPerStack = 1;
		Icon = "res://textures/resources/pickaxe.png";
		Type = ResourceTypes.Tool;
	}
}
