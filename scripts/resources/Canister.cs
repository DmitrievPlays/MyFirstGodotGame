public partial class Canister : Resource
{
	public Canister()
	{
		Name = "Canister";
		Description = "Canister for storing liquids";
		MaxPerStack = 16;
		Icon = "res://textures/resources/canister.png";
		Type = ResourceTypes.Resource;
	}
}