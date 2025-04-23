public class Slot
{
    public BaseItem BaseItem { get; set; }
    public int ItemAmount { get; set; }
    public SlotProperties Properties { get; init; }

    public Slot(BaseItem resource, int amount)
    {
        BaseItem = resource;
        ItemAmount = amount;
    }

    public Slot()
    {
    }
}
