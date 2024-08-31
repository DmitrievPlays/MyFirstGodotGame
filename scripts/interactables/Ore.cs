using Godot;

public partial class Ore : StaticBody3D
{
	protected float resourceAmount;
	
	public Ore(float initialAmount)
	{
		resourceAmount = initialAmount;
	}
	
	public virtual void Mine(float amount)
	{
		if (resourceAmount > 0)
			resourceAmount -= amount;

		if (resourceAmount == 0)
			QueueFree();
	}

	public float GetResourceAmount()
	{
		return resourceAmount;
	}
}
