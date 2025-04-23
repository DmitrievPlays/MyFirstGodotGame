using Godot;

public partial class SettingsScreen : PanelContainer
{
    [Export]
    private CheckButton showFps;

    [Export]
    private HSlider far;

    [Export]
    private OptionButton antialiasing;

    private Input.MouseModeEnum MouseModeBefore;

    public override void _Ready()
    {
        showFps.SetPressedNoSignal(bool.Parse(SettingsHandler.Instance.GetSetting("show_fps")));
        far.Value = (int.Parse(SettingsHandler.Instance.GetSetting("far")));
        antialiasing.Selected = int.Parse(SettingsHandler.Instance.GetSetting("rendering/anti_aliasing/quality/msaa_3d"));
    }

    public override void _Draw()
    {
        MouseModeBefore = Input.MouseMode;
        Input.MouseMode = Input.MouseModeEnum.Visible;

        showFps.SetPressedNoSignal(bool.Parse(SettingsHandler.Instance.GetSetting("show_fps")));
        far.Value = int.Parse(SettingsHandler.Instance.GetSetting("far"));
        antialiasing.Selected = int.Parse(SettingsHandler.Instance.GetSetting("rendering/anti_aliasing/quality/msaa_3d"));
    }

    public void Close()
    {
        Input.MouseMode = MouseModeBefore;
        SettingsHandler.Instance.ApplySettings();
        Hide();
    }

    private void OnAntialiasingChanged(int index)
    {
        ProjectSettings.SetSetting("rendering/anti_aliasing/quality/msaa_3d", index);
        SettingsHandler.Instance.PutSetting("rendering/anti_aliasing/quality/msaa_3d", index.ToString());
    }

    private void OnViewDistanceChanged(float value)
    {
        Label far = (Label)GetNode("MarginContainer/PanelContainer/MarginContainer/ScrollContainer/VBoxContainer/ViewDistance/HBoxContainer/HBoxContainer/ViewDistanceLabel");
        far.Text = $"({value.ToString()}m)";

        SettingsHandler.Instance.PutSetting("far", value.ToString());
    }

    private void OnShowFpsChanged(bool value)
    {
        SettingsHandler.Instance.PutSetting("show_fps", value.ToString());
    }
}
