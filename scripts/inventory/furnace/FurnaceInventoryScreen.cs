using Godot;

public partial class FurnaceInventoryScreen : Control
{
    public GridContainer content;
    private PackedScene inventoryItem;
    private bool visible = false;

    public override void _Ready()
    {
        content = (GridContainer)FindChild("Content");
        inventoryItem = ResourceLoader.Load<PackedScene>("res://InventoryItem.tscn");
    }

    public void ShowScreen(FurnaceInventory inventory)
    {
        if (visible)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            foreach (Node n in content.GetChildren())
                content.RemoveChild(n);

            visible = !visible;
            Visible = false;
            return;
        }

        visible = !visible;
        Input.MouseMode = Input.MouseModeEnum.Visible;
        var itemsToShow = inventory.GetItems();
        int itemsCount = itemsToShow.Count;

        for (int i = 1; i <= itemsCount; i++)
        {
            SlotInfo itemTemplate = inventoryItem.Instantiate<SlotInfo>();
            itemTemplate.Slot = itemsToShow[i];

            if (itemsToShow.ContainsKey(i) && itemsToShow[i].ItemAmount > 0)
            {
                ((TextureRect)itemTemplate.GetNode("Margin/Icon")).Texture = GD.Load(itemsToShow[i].Resource.Icon) as Texture2D;
                ((Label)itemTemplate.GetNode("Margin/Amount")).Text = itemsToShow[i].ItemAmount.ToString();
            }
            content.AddChild(itemTemplate);
        }

        Visible = true;
    }
}
