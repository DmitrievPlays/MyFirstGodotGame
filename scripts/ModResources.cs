﻿using Godot;
using System.Collections.Generic;

public class ModResources //MOD, BUT IT'S ORIGINAL
{
	public Dictionary<ResourceType, Resource> items = new Dictionary<ResourceType, Resource>() {
		{ ResourceType.WATER, new Water(){Name="Water", Description="A simple tasteless water!", MaxPerStack=1} },
		{ ResourceType.PICKAXE, new Pickaxe(){Name="Pickaxe", Description="You can mine resources faster with this", MaxPerStack=1} },
		{ ResourceType.FLASHLIGHT, new Flashlight(){Name="Flashlight", Description="With this tool you can reveal the secrets of the darkness", MaxPerStack=1} },
	};
	public enum ResourceType
	{
		WATER,
		PICKAXE,
		FLASHLIGHT
	}
}
