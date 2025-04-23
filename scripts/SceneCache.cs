using Godot;
using System;

public partial class SceneCache : Node
{
    private Dictionary<string, PackedScene> cachedScenes = new Dictionary<string, PackedScene>();

    public override void _Ready()
    {
        //CacheScene("PickupItem", "res://path_to_your_pickup_item_scene.tscn");
    }

    public void CacheScene(string key, string path)
    {
        if (!cachedScenes.ContainsKey(key))
        {
            PackedScene scene = GD.Load<PackedScene>(path);
            cachedScenes[key] = scene;
        }
    }

    public PackedScene GetCachedScene(string key)
    {
        return cachedScenes.ContainsKey(key) ? cachedScenes[key] : null;
    }
}
