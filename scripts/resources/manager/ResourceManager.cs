using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class ResourceManager : Button
{
	public static ResourceManager Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public override void _Pressed()
	{
		LoadResources();
		GD.Print("load");
	}

	public void LoadResources()
	{

		OriginalResources original = new();
		ModResources modResources = new();

		foreach (var item in original.items)
		{
			AddResource(item.Key, item.Value);
		}
		foreach (var item in modResources.items)
		{
			AddResource(item.Key, item.Value);
		}

		((Label)GetTree().Root.FindChild("LoadedContent", true, false)).Text = "";
		bool showingMods = false;

		foreach (var item in Resources)
		{
			if (item.Key > 254 && !showingMods)
			{
				((Label)GetTree().Root.FindChild("LoadedContent", true, false)).Text += "\n ---- Mods ----\n";
				showingMods = !showingMods;
			}
				
			((Label)GetTree().Root.FindChild("LoadedContent", true, false)).Text += item.Key + ", " + item.Value.GetType().FullName + "\n";
		}
	}



	public int ModResourceCap = 255; //Maximum items per mod

	public Dictionary<int, Resource> Resources = new(); //Resources main dictionary
	public Dictionary<string, int> Offsets = new(); //Resource ids offsets

	public async Task AddResource(Enum @enum, Resource resource) // Adds a single resource from an original list or a mod
	{
		var id = Convert.ToInt32(@enum);

		if (id >= ModResourceCap) // Maximum resources for one mod is 255!
			GD.PrintErr("Overflow max size");

		var offset = await GetOffset(@enum);

		var success = Resources.TryAdd(id + offset, resource);

		if (!success)
			GD.PrintErr("Resource already exists!");
	}

	public Task<int> GetOffset(Enum @enum)
	{
		var fullName = @enum.GetType().FullName;
		var exists = Offsets.TryGetValue(fullName, out var offset);

		if (!exists)
		{
			offset = Offsets.Count * ModResourceCap; //Getting ids offset depending on mod amount (3 mods = 0-254, 255-509, 510-764)
			Offsets.Add(fullName, offset); // Adding offset to cached list (the number indicates the first available id for the current mod)
		}

		return Task.FromResult(offset);
	}
}
