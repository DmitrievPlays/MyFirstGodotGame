using Godot;

public partial class GameServer : Node
{
    private ENetMultiplayerPeer peer;

    public override void _Ready()
    {
        peer = new ENetMultiplayerPeer();
    }

    public void OnCreateServerPressed()
    {
        peer.CreateServer(2025, 4);

        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
        {
            GD.PrintErr("Failed to start multiplayer server!");
            return;
        }
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("Server started on port: 2025");
    }

    public void OnConnectPressed()
    {
        var ip = ((TextEdit)GetTree().Root.FindChild("ip", true, false)).Text;
        var port = int.Parse(Mathf.RoundToInt(((SpinBox)GetTree().Root.FindChild("port", true, false)).Value).ToString());
        if (ip == "")
            return;

        peer.CreateClient(ip, port);

        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
        {
            GD.PrintErr("Failed to start multiplayer client");
            return;
        }
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("Client connected to: " + ip + ":" + port);
    }

    public override void _Process(double delta)
    {
        // Handle incoming packets
        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Connected)
        {
            // Process network events
            // e.g., handle player movement, game state updates, etc.
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            ExitDialog.Instance.ShowDialog(GetTree().Root);
        }
    }
}
