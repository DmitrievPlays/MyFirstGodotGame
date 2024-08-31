using Godot;

public partial class PlayerController : CharacterBody3D, ICollector
{
	private Inventory Inventory = new Inventory(maxSlots: 15);

	private int health = 100;

	[Export]
	public int Speed { get; set; } = 3;

	[Export]
	public int FallAcceleration { get; set; } = 75;

	private Vector3 _targetVelocity = Vector3.Zero;

	public override void _PhysicsProcess(double delta)
	{
		GD.Print("player pos: " + Position);
		var direction = Vector3.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1.0f;
		}
		if (Input.IsActionPressed("move_back"))
		{
			direction.Z += 1.0f;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 1.0f;
		}
		
		if (Input.IsActionPressed("jump"))
		{
			direction.Z -= 1.0f;
		}

		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			((CharacterBody3D)GetTree().Root.FindChild("Player", true, false)).Basis = Basis.LookingAt(direction);
		}


		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;


		if (!IsOnFloor())
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}

		Velocity = _targetVelocity;
		MoveAndSlide();
	}

	public void CollectItem(WorldResource item)
	{
		GD.Print("Collected: " + item.Name);
	}
}
