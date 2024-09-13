using Godot;
using System.Collections.Generic;
using System.Collections.Immutable;

public abstract class Inventory
{
	/// <summary>
	/// Does this inventory uses custom slots?
	/// </summary>
	public abstract bool HasCustomSlots();

	public virtual void SwapSlots(Slot slot1, Slot slot2)
	{
		var tempResource = slot1.Resource;
		var tempResourceAmount = slot1.ItemAmount;

		slot1.Resource = slot2.Resource;
		slot1.ItemAmount = slot2.ItemAmount;

		slot2.Resource = tempResource;
		slot2.ItemAmount = tempResourceAmount;
	}

	public abstract ImmutableDictionary<int, SlotProperties> getCustomSlotsProperties();

	/// <summary>
	/// Resource addition without selecting slot explicitly
	/// </summary>
	public abstract void AddItem(Resource resource, int amount);

	/// <summary>
	/// Resource removing without selecting slot explicitly
	/// </summary>
	public abstract void RemoveItem(Resource resource, int amount);

	/// <summary>
	/// Resource addition to slot with index
	/// </summary>
	public abstract void AddItemTo(int amount, int index);

	/// <summary>
	/// Resource removing from slot with index
	/// </summary>
	public abstract void RemoveItemFrom(int index);

	public virtual void DropItem(Slot slot, Vector3 head, Vector3 direction)
	{
		PickableItemsSpawner.Instance.SpawnPickable(slot, head, direction);
	}

	/// <summary>
	///  Get inventory slot by index
	/// </summary>
	public abstract int GetSlotByIndex(int index);

	/// <summary>
	/// Can this inventory contain this type of resource, OVERALL
	/// </summary>
	public abstract bool CanContain(Resource resource);

	/// <summary>
	/// If there is at least one not fulfilled slot for the resource collected
	/// </summary>
	public abstract bool CanCollect(Resource resource);

	/// <summary>
	/// Can this inventory contain this type of resource, BUT IN THIS SLOT
	/// </summary>
	public abstract bool CanContain(Resource resource, Slot slot);

	public abstract ImmutableDictionary<int, Slot> GetItems();
}