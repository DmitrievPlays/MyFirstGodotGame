using System;
using Godot;

public partial class PauseScreen : PanelContainer
{
    [Export]
    public Label version;

    private Input.MouseModeEnum MouseModeBefore;

    private bool isOpened = false;

    public override void _Ready()
    {
        version.Text = (string)ProjectSettings.GetSetting("application/config/version");
    }

    public void Resume()
    {
        HideMenu();
    }

    public void ShowMenu()
    {
        if (isOpened)
            return;

        Engine.TimeScale = 0;
        MouseModeBefore = Input.MouseMode;
        Input.MouseMode = Input.MouseModeEnum.Visible;

        Show();
        isOpened = true;
    }

    public void HideMenu()
    {
        Engine.TimeScale = 1;
        Input.MouseMode = MouseModeBefore;
        isOpened = false;
        Hide();
    }

    public void OpenSettings()
    {
        SettingsHandler.Instance.OpenSettingsScreen(GetTree().Root);
    }

    public void OpenLanSession()
    {
        LobbyHandler.Instance.OpenLobbyScreen(GetTree().Root);
    }

    public void SaveAndExit()
    {
        GetTree().ChangeSceneToFile("res://scenes/LandingPage.tscn");
        HideMenu();
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public override void _Input(InputEvent @event)
    {
        //if (Input.IsActionJustPressed("pause"))
        //HideMenu();
    }
}
