using Godot;

public partial class ExitDialog : Control
{
    private PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://scenes/ExitDialog.tscn");
    private ExitDialog _exitDialog;
    public static ExitDialog Instance { get; private set; }

    private Input.MouseModeEnum MouseModeBefore;

    private bool isAdded = false;

    public override void _Ready()
    {
        _exitDialog = (ExitDialog)packedScene.Instantiate();
        Instance = this;
    }

    public void ShowDialog(Node parent)
    {
        _exitDialog.Show();
        if (!isAdded)
        {
            parent.AddChild(_exitDialog);
            isAdded = true;
        }

        MouseModeBefore = Input.MouseMode;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    private void OnCancelPressed()
    {
        Input.MouseMode = MouseModeBefore;
        Hide();
    }

    private void OnLeavePressed()
    {
        GetTree().Quit(0);
    }
}
