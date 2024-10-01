using Godot;

public partial class WorldSelect : PanelContainer
{
    [Export]
    public ScrollContainer worldsHolder;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsKeyPressed(Key.Escape))
            Hide();
    }
}
