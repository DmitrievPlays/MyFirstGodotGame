using Godot;

public partial class Flashlight : Resource
{
	public override required string Name { get => base.Name; init => base.Name = value; }
	public override required string Description { get => base.Description; init => base.Description = value; }
	public override required float MaxPerStack { get => base.MaxPerStack; init => base.MaxPerStack = value; }
}
