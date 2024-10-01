public class StreetLight : Resource, IPlacable
{
    public StreetLight()
    {
        Name = "StreetLight";
        Description = "StreetLight!";
        MaxPerStack = 1;
        Icon = "res://textures/resources/streetlight.png";
        Type = ResourceTypes.Buildings;
    }

    public void Place()
    {
    }
}
