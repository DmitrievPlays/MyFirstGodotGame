using Godot;

public partial class Ore : StaticBody3D
{
	[Export]
	protected float resourceAmount;

	[Export]
	public int timeToMineOne;

	public double timeElapsed;

	public bool isInterating;

	public Ore(float initialAmount)
	{
		resourceAmount = initialAmount;
	}

	public virtual void Mine(float amount)
	{
		//if (resourceAmount > 0)
		//	resourceAmount -= amount;
	}

	public float GetResourceAmount()
	{
		return resourceAmount;
	}

	public override void _Process(double delta)
	{
		if (!isInterating) return;

		if (Input.IsActionPressed("Throw"))
			timeElapsed += delta;

		if (Input.IsActionJustReleased("Throw"))
			timeElapsed = 0;

		if (timeElapsed >= timeToMineOne)
		{
			resourceAmount--;
			timeElapsed = 0;
		}

		if (resourceAmount == 0)
			QueueFree();
	}

	public override void _MouseEnter()
	{
		isInterating = true;
	}

	public override void _MouseExit()
	{
		isInterating = false;
	}
}