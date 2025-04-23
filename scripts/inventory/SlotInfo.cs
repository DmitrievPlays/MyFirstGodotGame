using Godot;

public partial class SlotInfo : Button
{
    public int SlotID { get; set; }
    public Slot Slot { get; set; }

    public int ItemAmount => Slot.ItemAmount;

    private Tooltip tooltipEl;
    private Control UI;

    public override void _Ready()
    {
        UI = GetTree().Root.FindChild("MainUI", true, false) as Control;
        tooltipEl = GetTree().Root.FindChild("Tooltip", true, false) as Tooltip;
    }

    public void OnMouseEnter()
    {
        if (Slot?.BaseItem is null)
            return;
        tooltipEl.Visible = true;
        ((Label)tooltipEl.FindChild("name")).Text = Slot.BaseItem.Name;
        ((Label)tooltipEl.FindChild("description")).Text = Slot.BaseItem.Description;
        ((Label)tooltipEl.FindChild("id")).Text = "ID: " + Slot.BaseItem.Id.ToString();
        ((Label)tooltipEl.FindChild("slot_id")).Text = "SLOT_ID: " + SlotID.ToString();
    }

    public void OnMouseHover(InputEvent @event)
    {
        tooltipEl.Position = GetViewport().GetMousePosition();
    }

    public void OnMouseLeave()
    {
        tooltipEl.Visible = false;
    }
}
