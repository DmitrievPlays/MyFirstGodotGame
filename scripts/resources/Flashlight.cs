public partial class Flashlight : Resource
{
	public Flashlight()
	{
		Name = "Flashlight";
		Description = "You can reveal the secrets of darkness with this thing";
		MaxPerStack = 1;
		Icon = "res://textures/resources/flashlight.png";
		Type = ResourceTypes.Tool;
	}
}
