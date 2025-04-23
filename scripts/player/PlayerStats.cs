using Godot;

public partial class PlayerStats : PanelContainer
{
    [Export]
    public ProgressBar Health;

    [Export]
    public ProgressBar Food;

    [Export]
    public ProgressBar Water;

    public override void _Ready()
    {
        CurrentPlayerData.Instance.MyEvent += UpdateUI;
    }

    private void UpdateUI()
    {
        Health.Value = CurrentPlayerData.Instance.Health;
        Food.Value = CurrentPlayerData.Instance.Food;
        Water.Value = CurrentPlayerData.Instance.Water;
    }
}
