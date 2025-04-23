using Godot;
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
        int index = 1;

        for (var i = 0; i < 4; i++) // y
        {
            for (var j = 0; j < 8; j++) // x
            {
                inventory.Add(index, new Slot() { Properties = new(new Vector2(100 * j + 5 * j, 100 * i + 5 * i), 1) });
                index++;
            }
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
        OnChanged(this, EventArgs.Empty);
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
                return;
        }
        OnChanged(this, EventArgs.Empty);
    }

    public override bool CanContain(BaseItem resource) => true;

    public override bool CanContain(BaseItem resource, Slot slot) => true;

    public override bool CanCollect(BaseItem resource) => _slots.Values.Any(x => (x.BaseItem == resource && x.ItemAmount < resource.MaxPerStack) || x.BaseItem is null);

    public override ImmutableDictionary<int, Slot> GetItems() => _slots;

    public override bool HasCustomSlots()
    {
        return true;
    }

    public override void AddItem(BaseItem resource, int amount, int index)
    {
        _slots[index].BaseItem = resource;
        _slots[index].ItemAmount = amount;

        OnChanged(this, EventArgs.Empty);
    }

    public override Slot GetSlotByIndex(int index)
    {
        return _slots[index];
    }
}
