using Godot;
using Newtonsoft.Json;

public partial class LobbyHandler : Node
{
    private PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://scenes/lobby/lobby_screen.tscn");
    private LobbyScreen LobbyScreen;

    private bool isAdded = false;

    public static LobbyHandler Instance { get; private set; }

    public override void _Ready()
    {
        LobbyScreen = (LobbyScreen)packedScene.Instantiate();
        Instance = this;
    }

    public void OpenLobbyScreen(Node parent)
    {
        LobbyScreen.Show();

        if (!isAdded)
        {
            parent.AddChild(LobbyScreen);
            isAdded = true;
        }
    }
}
