using Godot;
using System.Collections.Generic;

public partial class InventoryManager : Node
{
    public Dictionary<string, Inventory> Inventories = new();

    public static InventoryManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;

        Inventories.Add("Player01", new PlayerInventory());
        Inventories.Add("Furnace01", new FurnaceInventory());
        Inventories.Add("Furnace02", new FurnaceInventory());
        Inventories.Add("CoalGenerator01", new CoalGeneratorInventory());
    }

    public Inventory GetPlayerInventory(string name)
    {
        if (name == "Current")
        {
            var currentPlayerName = GetViewport().GetCamera3D().GetNode("../../").Name;
            return Inventories[currentPlayerName];
        }

        return Inventories[name];
    }

    public Inventory GetBuildingInventory(string name)
    {
        return Inventories[name];
    }
}
