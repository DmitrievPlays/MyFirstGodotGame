using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    public PlayerInventory Inventory;

    private PackedScene screenPrefab;

    private CustomInventoryScreen screen;

    private InventoryHolderScreen inventoryHolder;

    [Export]
    private ObjectPlacer ObjectPlacer;

    [Export]
    private PlayerInventoryHotbar Hotbar; // player hotbar

    private int lastSelectedIndex;

    private BaseItem Hand; // player is now holding in hand

    [Export]
    public Node3D _playerHand; // player hand

    [Export]
    private Sprite3D _playerHandTexture; // player hand item

    public VBoxContainer playerStats;

    public override void _Ready()
    {
        playerStats = GetTree().Root.GetNode<VBoxContainer>("/root/MainScene/MainUI/PlayerStats/MarginContainer/VBoxContainer");

        Input.MouseMode = Input.MouseModeEnum.Captured;

        screenPrefab = ResourceLoader.Load<PackedScene>("res://prefabs/custom_screen.tscn");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("debug"))
        {
            var temp = GetTree().Root.GetNode<Control>("/root/MainScene/MainUI/Debug");
            temp.Visible = !temp.Visible;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("click"))
        {
            //if (Hand is not null)
            //Hand.Interact();
        }

        if (Input.IsActionJustPressed("inventory"))
        {
            if (inventoryHolder.Visible)
            {
                inventoryHolder.HideScreen();
                return;
            }

            screen = screenPrefab.Instantiate<CustomInventoryScreen>();
            screen.ScreenName = "Player01";
            screen.Inventory = Inventory;
        }

        if (Input.IsActionJustPressed("throw"))
        {
            if (Hand is null)
                return;

            if (Hand is IPlacable placable)
                ObjectPlacer.Activate(Hand);
        }

        if (Input.IsActionJustPressed("drop"))
        {
            //PlayerInventory.DropItem(Inventory.GetSlotByIndex(lastSelectedIndex + 1), -HEAD.GlobalTransform.Basis.Z.Normalized(), HEAD.GlobalTransform.Origin - HEAD.GlobalTransform.Basis.Z * 2);
            Inventory.RemoveItemFrom(lastSelectedIndex + 1);
        }
    }

    private void RenderHand(object sender, EventArgs e)
    {
        var selectedIndex = ((PlayerInventoryHotbar)sender).SelectionIndex + 1;
        Hand = Inventory.GetItems()[selectedIndex].BaseItem;
        _playerHandTexture.Texture = ResourceLoader.Load<Texture2D>(@$"{Hand?.Icon}");
    }
}
