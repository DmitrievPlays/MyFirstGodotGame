public class OriginalResources //MOD, BUT IT'S ORIGINAL
{
    public Dictionary<ResourceType, Resource> items = new Dictionary<ResourceType, Resource>() {
        { ResourceType.IRON, new IronBar() },
        { ResourceType.GOLD, new GoldBar() },
        { ResourceType.COPPER, new CopperBar() },
        { ResourceType.SILICON, new Silicon() },
        { ResourceType.COAL, new Coal() },
        { ResourceType.WATER_CAN, new Canister() },
        { ResourceType.STREETLIGHT, new StreetLight() },
        { ResourceType.WIRE, new Wire() },
    };

    public enum ResourceType
    {
        IRON,
        GOLD,
        COPPER,
        SILICON,
        COAL,
        WATER_CAN,
        STREETLIGHT,
        WIRE,
    }
}
