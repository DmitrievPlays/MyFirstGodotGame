using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public class PlayerInventory : Inventory
{
	public ImmutableDictionary<int, Slot> _slots { get; set; }

	public PlayerInventory()
	{
		_slots = CreateInventory();
	}

	private ImmutableDictionary<int, Slot> CreateInventory()
	{
		var inventory = new Dictionary<int, Slot>(32);

		for (var i = 1; i <= 32; i++)
		{
			inventory.Add(i, new Slot());
		}

		return inventory.ToImmutableDictionary();
	}

	public override void AddItem(Resource resource, int amount)
	{
		foreach (var slot in _slots.Values)
		{
			if (slot.Resource is null || slot.Resource.Id != resource.Id)
				continue;

			var difference = slot.Resource.MaxPerStack - slot.ItemAmount;
			var canBeInsertedAmount = difference < amount ? difference : amount;
			if (canBeInsertedAmount > 0)
			{
				slot.Resource = resource;
				slot.ItemAmount += canBeInsertedAmount;
				amount -= canBeInsertedAmount;
			}

			if (amount == 0)
				return;
		}

		foreach (var slot in _slots.Values)
		{
			if (slot.Resource is not null)
				continue;

			var canBeInsertedAmount = resource.MaxPerStack < amount ? resource.MaxPerStack : amount;
			slot.Resource = resource;
			slot.ItemAmount = canBeInsertedAmount;
			amount -= canBeInsertedAmount;

			if (amount == 0)
				return;
		}
	}

	public override void RemoveItem(Resource resource, int amount)
	{
		foreach (var slot in _slots.Values)
		{
			if (slot.Resource is null || slot.Resource.Id != resource.Id)
				continue;

			if (slot.ItemAmount > amount)
			{
				slot.ItemAmount -= amount;
			}
			else if (slot.ItemAmount <= amount)
			{
				amount -= slot.ItemAmount;
				slot.Resource = null;
				slot.ItemAmount = 0;
			}
			if (amount == 0)
				return;
		}
	}

	public override bool CanContain(Resource resource) => true;

	public override bool CanContain(Resource resource, Slot slot) => true;

	public override bool CanCollect(Resource resource) => _slots.Values.Any(x => (x.Resource == resource && x.ItemAmount < resource.MaxPerStack) || x.Resource is null);

	public override ImmutableDictionary<int, Slot> GetItems() => _slots;

	public override bool HasCustomSlots()
	{
		return false;
	}

	public override ImmutableDictionary<int, SlotProperties> getCustomSlotsProperties()
	{
		return null;
	}

	public override void AddItemTo(int amount, int index)
	{
		throw new System.NotImplementedException();
	}

	public override void RemoveItemFrom(int index)
	{
		_slots[index].Resource = null;
		_slots[index].ItemAmount = 0;
	}

	public override int GetSlotByIndex(int index)
	{
		throw new System.NotImplementedException();
	}
}