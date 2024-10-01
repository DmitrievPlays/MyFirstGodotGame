public class Slot
{
    public Resource Resource { get; set; }
    public int ItemAmount { get; set; }
    public SlotProperties Properties { get; init; }

    public Slot(Resource resource, int amount)
    {
        Resource = resource;
        ItemAmount = amount;
    }

    public Slot()
    {
    }
}
