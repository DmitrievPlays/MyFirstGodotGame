using System.Collections.Generic;

public class Inventory
{
	private Dictionary<int, Slot> InventorySlots { get; set; }
	private int InventorySlotCount { get; set; }

    public Inventory(int maxSlots)
    {
        InventorySlotCount = maxSlots;
    }

    public bool AddItem(int slotId, Resource resource, int amount)
	{
		if (InventorySlots.Count >= InventorySlotCount)
			return false;

		Slot slot = new Slot(resource, amount);
		InventorySlots.Add(slotId, slot);
		return true;
	}

	public void RemoveItem(int slotId)
	{
		InventorySlots.Remove(slotId);
	}
}
