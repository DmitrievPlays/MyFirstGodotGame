using Godot;
using System;
using System.Collections.Immutable;

public abstract class Inventory
{
    public delegate void NotifyEventHandler(object sender, EventArgs e);

    public event NotifyEventHandler OnInventoryChanged;

    public virtual void OnChanged(object sender, EventArgs e)
    {
        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Does this inventory uses custom slots?
    /// </summary>
    public abstract bool HasCustomSlots();

    public static void SwapSlots(Slot slot1, Slot slot2)
    {
        var tempBaseItem = slot1.BaseItem;
        var tempBaseItemAmount = slot1.ItemAmount;

        slot1.BaseItem = slot2.BaseItem;
        slot1.ItemAmount = slot2.ItemAmount;

        slot2.BaseItem = tempBaseItem;
        slot2.ItemAmount = tempBaseItemAmount;
    }

    /// <summary>
    /// BaseItem addition without selecting slot explicitly
    /// </summary>
    public abstract void AddItem(BaseItem BaseItem, int amount);

    /// <summary>
    /// BaseItem removing without selecting slot explicitly
    /// </summary>
    public abstract void RemoveItem(BaseItem BaseItem, int amount);

    /// <summary>
    /// BaseItem addition to slot with index
    /// </summary>
    public abstract void AddItem(BaseItem BaseItem, int amount, int index);

    /// <summary>
    /// BaseItem removing from slot with index
    /// </summary>
    public virtual void RemoveItemFrom(int slot)
    {
        GetItems()[slot].BaseItem = null;
        GetItems()[slot].ItemAmount = 0;
        // OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public static void DropItem(Slot slot, Vector3 head, Vector3 direction)
    {
        PickableItemsSpawner.Instance.SpawnPickable(slot, head, direction);
    }

    /// <summary>
    ///  Get inventory slot by index
    /// </summary>
    public abstract Slot GetSlotByIndex(int index);

    /// <summary>
    /// Can this inventory contain this type of BaseItem, OVERALL
    /// </summary>
    public abstract bool CanContain(BaseItem BaseItem);

    /// <summary>
    /// If there is at least one not fulfilled slot for the BaseItem collected
    /// </summary>
    public abstract bool CanCollect(BaseItem BaseItem);

    /// <summary>
    /// Can this inventory contain this type of BaseItem, BUT IN THIS SLOT
    /// </summary>
    public abstract bool CanContain(BaseItem BaseItem, Slot slot);

    public abstract ImmutableDictionary<int, Slot> GetItems();
}
