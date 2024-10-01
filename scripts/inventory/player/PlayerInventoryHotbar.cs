using System;
using Godot;

public partial class PlayerInventoryHotbar : Control
{
    public ResourceManager ResourceManager;

    public InventoryManager PlayerInventories;

    private PlayerInventory Inventory;

    private Control hotbar;

    public override void _Ready()
    {
        ResourceManager = ResourceManager.Instance;
        PlayerInventories = InventoryManager.Instance;
        Inventory = PlayerInventories.GetPlayerInventory("Player01") as PlayerInventory;
        hotbar = ((Control)GetNode("Margin/Content"));
    }

    public override void _Process(double delta)
    {
        int i = 0;
        foreach (var item in Inventory.GetItems().Values)
        {
            if (i > 7)
                return;

            if (item.Resource is not null)
                ((TextureRect)hotbar.GetChild(i).GetNode("Margin/Icon")).Texture = GD.Load(item.Resource.Icon) as Texture2D;
            else
                ((TextureRect)hotbar.GetChild(i).GetNode("Margin/Icon")).Texture = null;
            i++;
        }
    }

    public override void _Input(InputEvent @event)
    {
    }
}
