using Godot;

public partial class LandingPage : Node3D
{
    [Export]
    public Label version;

    [Export]
    public Camera3D mainCamera;

    private const float CAMERA_ROTATION_SPEED = 3;

    [Export]
    public SettingsScreen SettingsScreen;

    public override void _Ready()
    {
        version.Text = (string)ProjectSettings.GetSetting("application/config/version");
    }

    public override void _Process(double delta)
    {
        mainCamera.RotationDegrees += new Vector3(0, (float)delta * CAMERA_ROTATION_SPEED, 0);
    }

    public void Play()
    {
        GetTree().ChangeSceneToFile("res://scenes/main_scene.tscn");
    }

    public void Settings()
    {
        SettingsScreen.Show();
    }

    public void Quit()
    {
        GetTree().Quit();
    }
}
