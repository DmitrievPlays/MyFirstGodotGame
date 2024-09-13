using Godot;
using System;

public partial class Spawn : Node
{
	const float SPAWN_RANDOM = 5.0f;

	public override void _Ready()
	{
		if (!Multiplayer.IsServer())
			return;

		Multiplayer.PeerConnected += AddPlayer;
		Multiplayer.PeerDisconnected += RemovePlayer;

		// Spawn already connected players.

		foreach (int id in Multiplayer.GetPeers())
			AddPlayer(id);

		// Spawn the local player unless this is a dedicated server export.
		if (!OS.HasFeature("dedicated_server"))
			AddPlayer(1);
	}


	public override void _ExitTree()
	{
		if (!Multiplayer.IsServer())
		{
			Multiplayer.PeerConnected -= AddPlayer;
			Multiplayer.PeerDisconnected -= RemovePlayer;
		}

	}



	public void AddPlayer(long id)
	{
		var character = ResourceLoader.Load<PackedScene>("res://player.tscn").Instantiate() as PlayerController;
		
		// Set player id.
		character.player = id;

		// Randomize character position.
		var pos = Vector2.FromAngle(new Random().Next() * 2 * Mathf.Pi);
		character.Position = new Vector3(pos.X * SPAWN_RANDOM * new Random().Next(), 0, pos.Y * SPAWN_RANDOM * new Random().Next());
		character.Name = id.ToString();
		GetTree().Root.FindChild("Players", true, false).AddChild(character, true);
	}



	public void RemovePlayer(long id)
	{
		if (!GetTree().Root.FindChild("Players", true, false).HasNode(id.ToString()))
			return;
		GetTree().Root.FindChild("Players", true, false).GetNode(id.ToString()).QueueFree();
	}
}
