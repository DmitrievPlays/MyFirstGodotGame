using Godot;
using System.Collections.Immutable;

public partial class PlayerInventoryScreen : PanelContainer
{
	public GridContainer content;
	private PackedScene inventoryItem;

	public PlayerInventories PlayerInventories = PlayerInventories.Instance;
	private Node3D _node;
	public PlayerInventory thisInventory;
	public ImmutableDictionary<int, Slot> thisInventorySlots;

	public SlotInfo GrabbedItem { get; set; }
	public SlotInfo HoverOverItem { get; set; }
	public Tooltip tooltip;
	public SlotInfo hoveredItemPreview;
	private bool _isInFrame;

	public override void _Ready()
	{
		content = (GridContainer)FindChild("Content");
		inventoryItem = ResourceLoader.Load<PackedScene>("res://InventoryItem.tscn");
		tooltip = (Tooltip)GetTree().Root.FindChild("Tooltip", true, false);
		hoveredItemPreview = GetNode<Area2D>("MouseCursor").GetNode<SlotInfo>("InventoryItem");
	}

	public void ShowScreen(PlayerInventory inventory, Node3D node)
	{
		var resourceManager = ResourceManager.Instance;
		_node = node;

		thisInventory = inventory;
		thisInventorySlots = inventory.GetItems();

		if (Visible)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
			foreach (Node n in content.GetChildren())
				content.RemoveChild(n);

			Visible = !Visible;
			return;
		}

		Visible = !Visible;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		int itemsCount = thisInventorySlots.Count;

		RenderItems();
	}

	public void RenderItems()
	{
		foreach (Node n in content.GetChildren())
			content.RemoveChild(n);

		int itemsCount = thisInventory.GetItems().Count;

		for (int i = 1; i <= itemsCount; i++)
		{
			SlotInfo itemTemplate = inventoryItem.Instantiate<SlotInfo>();
			itemTemplate.Slot = thisInventorySlots[i];
			itemTemplate.SlotID = i;

			if (thisInventory.GetItems().ContainsKey(i) && thisInventory.GetItems()[i].ItemAmount > 0)
			{
				((TextureRect)itemTemplate.GetNode("Margin/Icon")).Texture = GD.Load(thisInventory.GetItems()[i].Resource.Icon) as Texture2D;
				((Label)itemTemplate.GetNode("Margin/Amount")).Text = thisInventory.GetItems()[i].ItemAmount.ToString();
			}
			if (thisInventory.HasCustomSlots())
				itemTemplate.Position = thisInventory.getCustomSlotsProperties()[i].SlotLocation;

			content.AddChild(itemTemplate);
		}
	}

	public override void _Input(InputEvent delta)
	{
		if (!Visible) return;

		GetNode<Area2D>("MouseCursor").Position = GetViewport().GetMousePosition();

		if (Input.IsActionJustPressed("Throw"))  // Clicked to put
		{
			if (GrabbedItem is not null)
			{
				if (HoverOverItem is not null)
				{
					thisInventory.SwapSlots(GrabbedItem.Slot, HoverOverItem.Slot);
					GrabbedItem = null;
				}
				else
				{
					if (_isInFrame)
						return;
					thisInventory.DropItem(GrabbedItem.Slot, -_node.GlobalTransform.Basis.Z.Normalized(), _node.GlobalTransform.Origin - _node.GlobalTransform.Basis.Z * 2);
					thisInventory.RemoveItemFrom(GrabbedItem.SlotID);
					GrabbedItem = null;
				}
				hoveredItemPreview.Hide();

				RenderItems();
			}
			else
			{
				if (HoverOverItem?.Slot?.Resource is not null)  // Clicked to take
				{
					GrabbedItem = HoverOverItem;
					hoveredItemPreview.Slot = GrabbedItem.Slot;
					((TextureRect)hoveredItemPreview.GetNode("Margin/Icon")).Texture = GD.Load(GrabbedItem.Slot?.Resource?.Icon) as Texture2D;
					hoveredItemPreview.Show();
				}
			}
		}
	}

	public void AreaEntered(Area2D area)
	{
		if (!Visible) return;

		Control button = area.GetParent<Control>();

		if (button is SlotInfo slot && slot?.Slot is not null)
			HoverOverItem = slot;
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