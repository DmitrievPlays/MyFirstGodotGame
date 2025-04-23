public class OriginalResources //MOD, BUT IT'S ORIGINAL
{
    public Dictionary<ResourceType, BaseItem> items = new()
    {
        //{ ResourceType.IRON, new IronBar() },
        //{ ResourceType.GOLD, new GoldBar() },
        //{ ResourceType.COPPER, new CopperBar() },
        //{ ResourceType.SILICON, new Silicon() },
        //{ ResourceType.COAL, new Coal() },
        //{ ResourceType.WATER_CAN, new Canister() },
        //{ ResourceType.STREETLIGHT, new StreetLight() },
        //{ ResourceType.WIRE, new Wire() },
        //{ ResourceType.GUN, new Gun() },
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
        GUN,
    }
}
