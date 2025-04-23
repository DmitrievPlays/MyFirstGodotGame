using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

public class WaterExtractorInventory : Inventory
{
    protected ImmutableDictionary<int, Slot> _slots { get; } = new Dictionary<int, Slot>()
    {
        { 1, new Slot() {Properties = new(new Vector2(40, 20), 1)}},
        { 2, new Slot() {Properties = new(new Vector2(170, 20), 2)}},
    }.ToImmutableDictionary();

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
    }

    public override bool CanCollect(BaseItem resource) => throw new NotImplementedException();

    public override bool CanContain(BaseItem resource) => throw new NotImplementedException();

    public override bool CanContain(BaseItem resource, Slot slot) => throw new NotImplementedException();

    public override ImmutableDictionary<int, Slot> GetItems() => _slots;

    public override bool HasCustomSlots()
    {
        return true;
    }

    public override void RemoveItem(BaseItem resource, int amount)
    {
    }

    public override void AddItem(BaseItem resource, int amount, int index)
    {
        throw new NotImplementedException();
    }

    public override Slot GetSlotByIndex(int index)
    {
        throw new NotImplementedException();
    }
}
