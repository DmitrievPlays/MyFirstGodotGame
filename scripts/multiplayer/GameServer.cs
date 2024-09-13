using Godot;
using System;

public partial class GameServer : Node
{
	private ENetMultiplayerPeer peer;

	public override void _Ready()
	{
		//GetTree().Paused = true;

		if (DisplayServer.GetName() == "headless")
		{
			GD.Print("Server is starting automatically...");
			CallDeferred(MethodName.OnHostPressed);
		}

		peer = new ENetMultiplayerPeer();

		//Multiplayer.MultiplayerPeer = peer;
	}

	public void OnHostPressed()
	{
		var peer = new ENetMultiplayerPeer();
		peer.CreateServer(2024, 16);

		if(peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
		{
			OS.Alert("Failed to start multiplayer server!");
			return;
		}
		Multiplayer.MultiplayerPeer = peer;
		StartGame();
	}

	public void OnConnectPressed()
	{
		var txt = ((TextEdit)GetTree().Root.FindChild("ip", true, false)).Text;
		var port = int.Parse(Mathf.RoundToInt(((SpinBox)GetTree().Root.FindChild("port", true, false)).Value).ToString());
		if(txt == "")
		{
			OS.Alert("No remote to connect to");
			return;
		}

		var peer = new ENetMultiplayerPeer();
		OS.Alert("Connecting to " + txt + ":" + port);
		peer.CreateClient(txt, port);

		if(peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
		{
			OS.Alert("Failed to start multiplayer client");
			return;
		}
		Multiplayer.MultiplayerPeer = peer;
		StartGame();
	}


	public void StartGame()
	{
		GetTree().Root.FindChild("ConnectUI", true, false).Set(Control.PropertyName.Visible, false);
		GetTree().Paused = false;
	}
}
