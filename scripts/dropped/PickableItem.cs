using Godot;

public partial class PickableItem : RigidBody3D
{
	[Export]
	public int Id;

	[Export]
	public int Amount;

	public string Icon;

	public void Prepare(int id, int itemAmount, string texturePath)
	{
		GetNode<Sprite3D>("Icon").Texture = GD.Load(texturePath) as Texture2D;
		Id = id;
		Amount = itemAmount;
		Icon = texturePath;
	}

	public void Pickup(PlayerInventory player, int id, int amount)
	{
		var resource = ResourceManager.Instance.ResourcesDatabase[id];
		var invManager = InventoryManager.Instance.GetPlayerInventory("Player01");
		invManager.AddItem(resource, amount);
		SoundManager.Instance.PlaySound("sonic-ring-sound", 1);
		QueueFree();
	}

	public void OnBodyEntered(Node body)
	{
		if (body is PlayerController player)
		{
			var resource = ResourceManager.Instance.ResourcesDatabase[Id];
			var invManager = InventoryManager.Instance.GetPlayerInventory("Player01");
			invManager.AddItem(resource, Amount);
			SoundManager.Instance.PlaySound("sonic-ring-sound", 1);
			QueueFree();
		}
	}
}