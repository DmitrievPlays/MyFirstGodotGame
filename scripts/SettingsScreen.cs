using Godot;

public partial class SettingsScreen : PanelContainer
{
    public override void _Ready()
    {
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsKeyPressed(Key.Escape))
            Close();
    }

    public void Close()
    {
        SettingsHandler.Instance.SaveSettingsToFile();
        Hide();
    }
}
