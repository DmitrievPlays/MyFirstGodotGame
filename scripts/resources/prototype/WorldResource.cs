using Godot;

public abstract partial class WorldResource : Area3D
{
    public delegate void CollectedEventHandler();

    public event CollectedEventHandler Collected;

    // This function is called when another object enters the collision area

    private void OnBodyEntered(Node body)
    {
        GD.Print("Entered");
        // Assuming the body is a Player or any other type of object that can collect items
        if (body is ICollector collector)
        {
            collector.CollectItem(this);
            Collected?.Invoke();
            QueueFree(); // Removes the collectible from the scene
        }
    }

    public override void _Ready()
    {
        //Connect("body_entered", new Callable(this, MethodName.OnBodyEntered));
    }
}
