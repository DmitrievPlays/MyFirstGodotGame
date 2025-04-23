using Godot;

public partial class Fps : Label
{
    private double _deltaTime;
    private int _frameCount;

    public override void _Process(double delta)
    {
        if (!bool.Parse(SettingsHandler.Instance.GetSetting("show_fps")))
        {
            Visible = false;
            return;
        }
        Visible = true;

        _deltaTime += delta;
        _frameCount++;

        if (_deltaTime >= 0.25) // Update every 1/4 second
        {
            int fps = (int)(_frameCount / _deltaTime);
            Text = $"FPS: {fps}";
            _deltaTime = 0;
            _frameCount = 0;

            if (fps > 48)
            {
                AddThemeColorOverride("font_color", Color.Color8(0, 255, 0));
            }
            else if (fps <= 48 && fps > 24)
            {
                AddThemeColorOverride("font_color", Color.Color8(255, 255, 0));
            }
            else if (fps <= 24)
            {
                AddThemeColorOverride("font_color", Color.Color8(255, 0, 0));
            }
        }
    }
}
