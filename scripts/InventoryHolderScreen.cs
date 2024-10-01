using System;
using Godot;

public partial class InventoryHolderScreen : PanelContainer
{
    [Export]
    private VBoxContainer _frame;

    private PackedScene _customScreenPrefab;

    public SlotInfo GrabbedItem { get; set; }
    public SlotInfo HoverOverItem { get; set; }
    public Inventory HoverOverInventory { get; set; }
    public Inventory GrabbedItemInventory { get; set; }

    public SlotInfo _hoveredItemPreview;

    private Node _screenOwner;

    private bool _isInFrame;

    public override void _Ready()
    {
        _customScreenPrefab = ResourceLoader.Load<PackedScene>("res://custom_screen.tscn");
        _hoveredItemPreview = GetNode<Area2D>("MouseCursor").GetNode<SlotInfo>("InventoryItem");
    }

    public void ShowScreen(CustomInventoryScreen[] screens, Node owner)
    {
        _screenOwner = owner;
        foreach (var screen in screens)
            _frame.AddChild(screen);

        Input.MouseMode = Input.MouseModeEnum.Visible;
        Show();
    }

    public void HideScreen()
    {
        _screenOwner = null;
        var children = _frame.GetChildren();
        foreach (var child in children)
        {
            //children.Remove(child);
            //child.CallDeferred("queue_free");
            //child.CallDeferred("free");
            _frame.RemoveChild(child);
            child.Free();
        }

        Input.MouseMode = Input.MouseModeEnum.Captured;
        Hide();
    }

    public override void _Input(InputEvent @event)
    {
        GetNode<Area2D>("MouseCursor").Position = GetViewport().GetMousePosition();

        if (Input.IsActionJustPressed("drop")) // Throw a hovered item from inventory
        {
            if (HoverOverItem is not null)
            {
                if (_isInFrame)
                    return;

                Inventory.DropItem(HoverOverItem.Slot, -_screenOwner.GetNode<Node3D>("Head").GlobalTransform.Basis.Z.Normalized(), _screenOwner.GetNode<Node3D>("Head").GlobalTransform.Origin - _screenOwner.GetNode<Node3D>("Head").GlobalTransform.Basis.Z * 2);
                HoverOverInventory.RemoveItemFrom(HoverOverItem.SlotID);
                GrabbedItem = null;

                HoverOverInventory.OnChanged(this, EventArgs.Empty);
                if (HoverOverInventory != GrabbedItemInventory)
                    GrabbedItemInventory.OnChanged(this, EventArgs.Empty);
            }
        }

        if (Input.IsActionJustPressed("throw"))  // Clicked to put
        {
            if (GrabbedItem is not null)
            {
                if (HoverOverItem is not null)
                {
                    HoverOverInventory.AddItem(GrabbedItem.Slot.Resource, GrabbedItem.ItemAmount, HoverOverItem.SlotID);
                    GrabbedItemInventory.RemoveItemFrom(GrabbedItem.SlotID);
                    //Inventory.SwapSlots(GrabbedItem.Slot, HoverOverItem.Slot);
                    GrabbedItem = null;
                }
                else
                {
                    if (_isInFrame)
                        return;

                    Inventory.DropItem(GrabbedItem.Slot, -((Node3D)_screenOwner).GlobalTransform.Basis.Z.Normalized(), ((Node3D)_screenOwner).GlobalTransform.Origin - ((Node3D)_screenOwner).GlobalTransform.Basis.Z * 2);
                    HoverOverInventory.RemoveItemFrom(GrabbedItem.SlotID);
                    GrabbedItem = null;
                }
                _hoveredItemPreview.Hide();

                HoverOverInventory.OnChanged(this, EventArgs.Empty);
                if (HoverOverInventory != GrabbedItemInventory)
                    GrabbedItemInventory.OnChanged(this, EventArgs.Empty);
            }
            else
            {
                if (HoverOverItem?.Slot?.Resource is not null)  // Clicked to take
                {
                    GrabbedItem = HoverOverItem;
                    GrabbedItemInventory = HoverOverInventory;
                    _hoveredItemPreview.Slot = GrabbedItem.Slot;
                    ((TextureRect)_hoveredItemPreview.GetNode("Margin/Icon")).Texture = GD.Load(GrabbedItem.Slot?.Resource?.Icon) as Texture2D;
                    _hoveredItemPreview.Show();

                    HoverOverInventory.OnChanged(this, EventArgs.Empty);
                    if (HoverOverInventory != GrabbedItemInventory)
                        GrabbedItemInventory.OnChanged(this, EventArgs.Empty);
                }
            }
        }
    }

    public void AreaEntered(Area2D area)
    {
        if (!Visible) return;

        Control hovered = area.GetParent<Control>();

        if (hovered is CustomInventoryScreen screen && screen.Inventory is not null)
            HoverOverInventory = screen.Inventory;

        if (hovered is SlotInfo slot && slot?.Slot is not null)
            HoverOverItem = hovered as SlotInfo;
    }

    public void AreaExited(Area2D area)
    {
        HoverOverItem = null;
    }

    public void InTheFrame()
    {
        _isInFrame = true;
    }

    public void OutTheFrame()
    {
        _isInFrame = false;
    }
}
