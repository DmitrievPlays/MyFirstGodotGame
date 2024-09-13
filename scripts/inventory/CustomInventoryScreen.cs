using Godot;
using System.Collections.Generic;

public partial class CustomInventoryScreen : PanelContainer
{
	public Control content;
	private PackedScene inventoryItem;
	private bool visible = false;

	public Vector2 lastMousePos;

	public List<SlotInfo> slotInfos = new();

	public SlotInfo GrabbedItem { get; set; }
	public SlotInfo HoverOverItem { get; set; }
	public Tooltip tooltip;
	public Label screenName;

	public override void _Ready()
	{
		content = (Control)FindChild("Anchor", true, false);
		inventoryItem = ResourceLoader.Load<PackedScene>("res://InventoryItem.tscn");
		tooltip = (Tooltip)GetTree().Root.FindChild("Tooltip", true, false);
		screenName = GetNode<Label>("PanelContainer/ScreenName");
	}

	public void ShowScreen(Inventory inventory, string screenName)
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
			itemTemplate.SlotID = i;

			if (itemsToShow.ContainsKey(i) && itemsToShow[i].ItemAmount > 0)
			{
				((TextureRect)itemTemplate.GetNode("Margin/Icon")).Texture = GD.Load(itemsToShow[i].Resource.Icon) as Texture2D;
				((Label)itemTemplate.GetNode("Margin/Amount")).Text = itemsToShow[i].ItemAmount.ToString();
			}
			if (inventory.HasCustomSlots())
				itemTemplate.Position = inventory.getCustomSlotsProperties()[i].SlotLocation;

			content.AddChild(itemTemplate);
		}

		this.screenName.Text = screenName;
		Visible = true;
	}

	public override void _Process(double delta)
	{
		if (!visible) return;

		GetNode<Area2D>("MouseCursor").Position = GetTree().Root.GetMousePosition() - new Vector2(320, 200);

		if (HoverOverItem != null)
		{
			if (Input.IsActionJustPressed("Throw"))
			{
				GrabbedItem = HoverOverItem;
				lastMousePos = GetTree().Root.GetMousePosition() - new Vector2(320, 200);
			}

			if (lastMousePos.DistanceTo(GetTree().Root.GetMousePosition() - new Vector2(320, 200)) > 2)
			{
				if (Input.IsActionPressed("Throw"))
				{
					SlotInfo button = GetNode<Area2D>("MouseCursor").GetNode<SlotInfo>("InventoryItem");
					button.Slot = GrabbedItem.Slot;
					button.Show();
					tooltip.Hide();
				}
				if (Input.IsActionJustReleased("Throw"))
				{
					//int buttonIndex = GrabbedItem.SlotID;
					//int button2Index = HoverOverItem.SlotID;

					//content.MoveChild(GrabbedItem, button2Index);
					//content.MoveChild(HoverOverItem, buttonIndex);

					SlotInfo button = GetNode<Area2D>("MouseCursor").GetNode<SlotInfo>("InventoryItem");
					button.Hide();
					tooltip.Show();
				}
			}
		}
	}

	public void AreaEntered(Area2D area)
	{
		if (!visible) return;

		Control button = area.GetParent<Control>();
		if (button is SlotInfo)
			HoverOverItem = button as SlotInfo;
	}

	//public void AreaExited(Area2D area) => HoverOverItem = null;
}