using Godot;

public partial class PickableItemsSpawner : Node
{
    public PackedScene itemToSpawnTemplate;
    public PickableItem itemToSpawn;

    public static PickableItemsSpawner Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        itemToSpawnTemplate = ResourceLoader.Load<PackedScene>("res://prefabs/Pickable.tscn");
    }

    public void SpawnPickable(Slot slot, Vector3 head, Vector3 direction)
    {
        PickableItem itemTemplate = itemToSpawnTemplate.Instantiate<PickableItem>();

        itemTemplate.Prepare(slot.BaseItem.Id, slot.ItemAmount, slot.BaseItem.Icon);
        itemTemplate.Position = direction;
        itemTemplate.ApplyImpulse(head.Normalized() * 4, Vector3.Zero);

        GetTree().Root.AddChild(itemTemplate);
    }
}
