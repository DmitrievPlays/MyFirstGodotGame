using System.Collections.Immutable;
using Godot;

public partial class CustomInventoryScreen : PanelContainer
{
    [Export] private CollisionShape2D _collisionBox;

    private Control _contentNode;
    private Vector2 _collisionBoxSize;
    private PackedScene _inventoryItem;
    private ImmutableDictionary<int, Slot> _inventorySlots;
    private Tooltip _tooltip;
    private Label _screenNameNode;
    private bool _isInFrame;
    private bool _isFirstRender = true;

    public Inventory Inventory { get; set; }
    public SlotInfo GrabbedItem { get; set; }
    public SlotInfo HoverOverItem { get; set; }
    public string ScreenName { get; set; }

    public override void _Ready()
    {
        _inventorySlots = Inventory.GetItems();
        Inventory.OnInventoryChanged += RenderItems;

        _contentNode = GetNode<Control>("Content/Anchor");

        _screenNameNode = GetNode<Label>("ScreenName");

        _inventoryItem = ResourceLoader.Load<PackedScene>("res://InventoryItem.tscn");
        _tooltip = (Tooltip)GetTree().Root.FindChild("Tooltip", true, false);
        _collisionBoxSize = Size;

        RenderItems(this, EventArgs.Empty);
    }

    public void RectChanged()
    {
        _collisionBox.Shape.Set(PropertyName.Size, new Vector2(Size.X, Size.Y));
        _collisionBox.Position = new Vector2(Size.X / 2, Size.Y / 2);
    }

    public void RenderItems(object sender, EventArgs e)
    {
        var itemsCount = Inventory.GetItems().Count;

        var children = _contentNode.GetChildren().Cast<SlotInfo>().ToList();

        for (var i = 1; i <= itemsCount; i++)
        {
            SlotInfo itemTemplate;

            if (_isFirstRender)
                itemTemplate = _inventoryItem.Instantiate<SlotInfo>();
            else
                itemTemplate = children[i - 1];

            itemTemplate.Slot = _inventorySlots[i];
            itemTemplate.SlotID = i;

            if (_inventorySlots[i].Resource is not null)
            {
                ((TextureRect)itemTemplate.GetNode("Margin/Icon")).Texture = GD.Load(_inventorySlots[i].Resource.Icon) as Texture2D;
                ((Label)itemTemplate.GetNode("Margin/Amount")).Text = _inventorySlots[i].ItemAmount.ToString();
            }
            else
            {
                ((TextureRect)itemTemplate.GetNode("Margin/Icon")).Texture = null;
                ((Label)itemTemplate.GetNode("Margin/Amount")).Text = "";
            }

            if (Inventory.HasCustomSlots())
                itemTemplate.Position = _inventorySlots[i].Properties.Location;

            if (_isFirstRender)
                _contentNode.AddChild(itemTemplate);
        }
        _isFirstRender = false;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Inventory.OnInventoryChanged -= RenderItems;
        }
        base.Dispose(disposing);
    }
}
