using Godot;

public partial class LobbyScreen : Panel
{
    public void Create()
    {
        (GetTree().Root.GetNode("MainScene") as GameServer).OnCreateServerPressed();
    }

    public void Connect()
    {
        (GetTree().Root.GetNode("MainScene") as GameServer).OnConnectPressed();
    }

    public void Close()
    {
        Hide();
    }
}
