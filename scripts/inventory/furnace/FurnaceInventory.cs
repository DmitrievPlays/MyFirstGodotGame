using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

public class FurnaceInventory : Inventory
{
    public enum SlotTypes
    {
        Input,
        Output,
    }

    protected ImmutableDictionary<int, Slot> _slots { get; } = new Dictionary<int, Slot>()
    {
        { 1, new Slot() { Properties = new(new Vector2(40, 20), 1) } }, //Input top (to melt)
		{ 2, new Slot() { Properties = new(new Vector2(40, 150), 1) } }, //Input bottom (to make fire)
		{ 3, new Slot() { Properties = new(new Vector2(180, 95), 2) } },  //Output
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

    public override void AddItem(BaseItem resource, int amount)
    {
        foreach (var slot in _slots.Values)
        {
            if (slot.BaseItem is null || slot.BaseItem.Id != resource.Id)
                continue;

            var difference = slot.BaseItem.MaxPerStack - slot.ItemAmount;
            var canBeInsertedAmount = difference < amount ? difference : amount;
            if (canBeInsertedAmount > 0)
            {
                slot.BaseItem = resource;
                slot.ItemAmount += canBeInsertedAmount;
                amount -= canBeInsertedAmount;
            }

            if (amount == 0)
                return;
        }

        foreach (var slot in _slots.Values)
        {
            if (slot.BaseItem is not null)
                continue;

            var canBeInsertedAmount = resource.MaxPerStack < amount ? resource.MaxPerStack : amount;
            slot.BaseItem = resource;
            slot.ItemAmount = canBeInsertedAmount;
            amount -= canBeInsertedAmount;

            if (amount == 0)
                return;
        }
        //OnChanged(this, EventArgs.Empty);
    }

    public override bool CanCollect(BaseItem resource) => throw new NotImplementedException();

    public override bool CanContain(BaseItem resource, Slot slot) => throw new NotImplementedException();

    public override bool CanContain(BaseItem resource) => throw new NotImplementedException();

    public override ImmutableDictionary<int, Slot> GetItems() => _slots;

    public override bool HasCustomSlots()
    {
        return true;
    }

    public override void RemoveItem(BaseItem resource, int amount)
    {
        foreach (var slot in _slots.Values)
        {
            if (slot.BaseItem is null || slot.BaseItem.Id != resource.Id)
                continue;

            if (slot.ItemAmount > amount)
            {
                slot.ItemAmount -= amount;
            }
            else if (slot.ItemAmount <= amount)
            {
                amount -= slot.ItemAmount;
                slot.BaseItem = null;
                slot.ItemAmount = 0;
            }
            if (amount == 0)
                break;
        }
        //OnChanged(this, EventArgs.Empty);
    }

    public override void AddItem(BaseItem resource, int amount, int index)
    {
        _slots[index].BaseItem = resource;
        _slots[index].ItemAmount = amount;
        //OnChanged(this, EventArgs.Empty);
    }

    public override Slot GetSlotByIndex(int index)
    {
        return _slots[index];
    }
}
