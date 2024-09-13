using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

public class FurnaceInventory : Inventory
{
	protected ImmutableDictionary<int, Slot> _slots { get; } = new Dictionary<int, Slot>()
	{
		{ 1, new Slot() },
		{ 2, new Slot() },
		{ 3, new Slot() },
	}.ToImmutableDictionary();

	public ImmutableDictionary<int, SlotProperties> _slotProperties { get; } = new Dictionary<int, SlotProperties>()
	{
		{1, new SlotProperties(){SlotLocation = new Vector2(40, 20), SlotType = 1} },
		{2, new SlotProperties(){SlotLocation = new Vector2(40, 150), SlotType = 1} },
		{3, new SlotProperties(){SlotLocation = new Vector2(180, 95), SlotType = 2} },
	}.ToImmutableDictionary();

	private ImmutableDictionary<int, Slot> CreateInventory()
	{
		var inventory = new Dictionary<int, Slot>(3);

		for (var i = 1; i <= 3; i++)
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

	public override bool CanCollect(Resource resource) => throw new NotImplementedException();

	public override bool CanContain(Resource resource, Slot slot) => throw new NotImplementedException();

	public override bool CanContain(Resource resource) => throw new NotImplementedException();

	public override ImmutableDictionary<int, Slot> GetItems() => _slots;

	public override bool HasCustomSlots()
	{
		return true;
	}

	public override ImmutableDictionary<int, SlotProperties> getCustomSlotsProperties()
	{
		return _slotProperties;
	}

	public override void RemoveItem(Resource resource, int amount)
	{
	}

	public override void AddItemTo(int amount, int index)
	{
		throw new NotImplementedException();
	}

	public override void RemoveItemFrom(int index)
	{
		throw new NotImplementedException();
	}

	public override int GetSlotByIndex(int index)
	{
		throw new NotImplementedException();
	}
}