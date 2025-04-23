using Godot;

public partial class PauseHandler : Node
{
    private PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://scenes/pause/pause_screen.tscn");
    private PauseScreen PauseScreen;

    private bool isAdded = false;

    public static PauseHandler Instance { get; private set; }

    public override void _Ready()
    {
        PauseScreen = (PauseScreen)packedScene.Instantiate();
        Instance = this;
    }

    public void OpenPauseScreen(Node parent)
    {
        PauseScreen.ShowMenu();

        if (!isAdded)
        {
            parent.AddChild(PauseScreen);
            isAdded = true;
        }
    }
}
