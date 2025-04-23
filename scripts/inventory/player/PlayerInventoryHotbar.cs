using Godot;

public partial class PlayerInventoryHotbar : Control
{
    public ResourceManager ResourceManager;

    public InventoryManager PlayerInventories;

    private PlayerInventory Inventory;

    private Control hotbar;

    public int SelectionIndex { get; set; }

    public delegate void NotifyEventHandler(object sender, EventArgs e);

    public event NotifyEventHandler IndexChanged;

    public virtual void OnIndexChanged(object sender, EventArgs e)
    {
        IndexChanged?.Invoke(this, EventArgs.Empty);
    }

    public override void _Ready()
    {
        hotbar = ((Control)GetNode("Margin/Content"));

        ResourceManager = ResourceManager.Instance;
        PlayerInventories = InventoryManager.Instance;

        Inventory = PlayerInventories.GetPlayerInventory("Player01") as PlayerInventory;
        Inventory.OnInventoryChanged += Update;
    }

    private void Update(object sender, EventArgs e)
    {
        int i = 0;
        foreach (var item in Inventory.GetItems().Values)
        {
            if (i > 7)
                break;

            if (item.BaseItem is not null)
                ((TextureRect)hotbar.GetChild(i).GetNode("Margin/Icon")).Texture = GD.Load(item.BaseItem.Icon) as Texture2D;
            else
                ((TextureRect)hotbar.GetChild(i).GetNode("Margin/Icon")).Texture = null;
            i++;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton emb && emb.IsPressed())
        {
            if (emb.ButtonIndex == MouseButton.WheelUp)
            {
                if (SelectionIndex > 6)
                    return;
                SelectionIndex++;
            }

            if (emb.ButtonIndex == MouseButton.WheelDown)
            {
                if (SelectionIndex <= 0)
                    return;
                SelectionIndex--;
            }

            SetIndex(SelectionIndex);
        }

        if (@event is InputEventKey key && key.Pressed)
        {
            int index = (int)key.Keycode - 49;
            if (key.Keycode >= Key.Key0 && key.Keycode < Key.Key9)
                SetIndex(index);
        }
    }

    public void SetIndex(int index)
    {
        SelectionIndex = index;

        foreach (var child in hotbar.GetChildren())
            child.Set(Button.PropertyName.Disabled, false);
        hotbar.GetChild(SelectionIndex).Set(Button.PropertyName.Disabled, true);

        OnIndexChanged(null, EventArgs.Empty);
    }
}
