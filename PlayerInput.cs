using Godot;
using System;

public partial class PlayerInput : MultiplayerSynchronizer
{
// Set via RPC to simulate is_action_just_pressed.
	[Export]
	bool jumping = false;

	// Synchronized property.
	[Export]
	Vector2 direction;


	public override void _Ready()
	{
		// Only process for the local player.
		SetProcess(GetMultiplayerAuthority() == Multiplayer.GetUniqueId());
	}


	[Rpc(CallLocal = true)]
	public void Jump()
	{
		jumping = true;
	}


	public override void _Process(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.

		direction = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		if (Input.IsActionJustPressed("jump"))
			Rpc(MethodName.Jump);
	}
}
