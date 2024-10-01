using Godot;

public partial class ObjectPlacer : Control
{
    [Export]
    private Label _mainTitle;

    public override void _Ready()
    {
        _mainTitle = GetNode<Label>("HFlowContainer/Label");
    }

    public override void _Input(InputEvent @event)
    {
        //Vector2 mousePosition = ((InputEventMouseButton)@event).Position;

        //Camera3D camera = (Camera3D)GetTree().Root.FindChild("Camera3D", true, false);
        //Vector3 from = camera.ProjectRayOrigin(mousePosition);
        //Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 4;

        //var spaceState = GetWorld3D().DirectSpaceState;
        //var query = PhysicsRayQueryParameters3D.Create(from, to);
        //query.CollideWithAreas = true;
        //var result = spaceState.IntersectRay(query);

        //if (result.Count > 0)
        //{
        //    if (result.TryGetValue("collider", out var colliderVariant))
        //    {
        //        Node collider = (Node)colliderVariant;
        //        if (collider is Ore miningable)
        //            miningable.Mine(1);
        //    }
        //}
    }

    public void Activate(Resource placable)
    {
        _mainTitle.Text = @$"Placing {placable.Name}";
        Show();
    }
}
