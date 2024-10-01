public class Wire : Resource, IPlacable
{
    public Wire()
    {
        Name = "Wire";
        Description = "Make these machines run!";
        MaxPerStack = 50;
        Icon = "res://textures/resources/wire.png";
        Type = ResourceTypes.Buildings;
    }

    public void Place()
    {
    }
}
