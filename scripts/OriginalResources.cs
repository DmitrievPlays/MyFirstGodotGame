using Godot;
using System.Collections.Generic;

public class OriginalResources //MOD, BUT IT'S ORIGINAL
{
	public Dictionary<ResourceType, Resource> items = new Dictionary<ResourceType, Resource>() {
		{ ResourceType.IRON, new IronBar() },
		{ ResourceType.GOLD, new GoldBar() },
		{ ResourceType.COPPER, new CopperBar() },
		{ ResourceType.SILICON, new Silicon() },
	};

	public enum ResourceType
	{
		IRON,
		GOLD,
		COPPER,
		SILICON,
	}
}