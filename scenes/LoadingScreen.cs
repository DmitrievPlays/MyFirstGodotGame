using Godot;

public partial class LoadingScreen : PanelContainer
{
    [Export]
    public ProgressBar ProgressBar;

    [Export]
    public TextureRect LoadingSpinner;

    Godot.Collections.Array progress = [];

    public override void _Ready()
    {
        ResourceLoader.LoadThreadedRequest("res://scenes/main_scene.tscn");
    }

    public override void _Process(double delta)
    {
        LoadingSpinner.Rotation += (float)delta * 4;

        var loading_status = ResourceLoader.LoadThreadedGetStatus(
            "res://scenes/main_scene.tscn",
            progress
        );

        switch (loading_status)
        {
            case ResourceLoader.ThreadLoadStatus.InProgress:
                ProgressBar.Value = (int)progress[0] * 100;
                break;
            case ResourceLoader.ThreadLoadStatus.Loaded:
                GetTree()
                    .ChangeSceneToPacked(
                        ResourceLoader.LoadThreadedGet("res://scenes/main_scene.tscn")
                            as PackedScene
                    );
                break;

            case ResourceLoader.ThreadLoadStatus.Failed:
                GD.Print("Error. Could not load Resource");
                break;
        }
    }
}
