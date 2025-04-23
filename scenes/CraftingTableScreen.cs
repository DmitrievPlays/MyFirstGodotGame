using System;
using Godot;

public partial class CraftingTableScreen : PanelContainer
{
    public override void _Ready() { }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsKeyPressed(Key.Escape))
            Hide();
    }
}
