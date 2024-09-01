using Godot;

public partial class Water : Resource
{
	public override string Name { get => base.Name; init => base.Name = value; }
	public override string Description { get => base.Description; init => base.Description = value; }
	public override float MaxPerStack { get => base.MaxPerStack; init => base.MaxPerStack = value; }
}
