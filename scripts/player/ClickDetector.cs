using Godot;

public partial class ClickDetector : Camera3D
{
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left } eventMouseButton)
        {
            Vector2 mousePosition = eventMouseButton.Position;

            PlayerController player = GetNode("../../") as PlayerController;
            Camera3D camera = (Camera3D)GetTree().Root.FindChild("Camera3D", true, false);
            Vector3 from = camera.ProjectRayOrigin(mousePosition);
            Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 4;

            var spaceState = GetWorld3D().DirectSpaceState;
            var query = PhysicsRayQueryParameters3D.Create(from, to);
            query.CollideWithAreas = true;
            var result = spaceState.IntersectRay(query);

            if (result.Count > 0)
            {
                if (result.TryGetValue("collider", out var colliderVariant))
                {
                    Node collider = (Node)colliderVariant;
                    if (collider is Ore miningable)
                        miningable.Mine(1);

                    if (collider is PowerNode powerNode)
                        powerNode.Interact();
                }
            }
        }

        if (Input.IsActionJustPressed("interact"))
        {
            Vector2 mousePosition = GetViewport().GetMousePosition();

            PlayerController player = GetNode("../../") as PlayerController;
            Camera3D camera = (Camera3D)GetTree().Root.FindChild("Camera3D", true, false);
            Vector3 from = camera.ProjectRayOrigin(mousePosition);
            Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 4;

            var spaceState = GetWorld3D().DirectSpaceState;
            var query = PhysicsRayQueryParameters3D.Create(from, to);
            query.CollideWithAreas = true;
            var result = spaceState.IntersectRay(query);

            if (result.Count > 0)
            {
                if (result.TryGetValue("collider", out var colliderVariant))
                {
                    Node collider = (Node)colliderVariant;

                    if (collider is Machine machine)
                        machine.OnInteract();

                    if (collider is PickableItem pickable)
                        pickable.Pickup(player.Inventory, pickable.Id, pickable.Amount);
                }
            }
        }
    }
}
