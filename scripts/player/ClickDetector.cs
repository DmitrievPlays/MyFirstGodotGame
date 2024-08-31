using Godot;

public partial class ClickDetector : Camera3D
{
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton
			&& eventMouseButton.Pressed
			&& eventMouseButton.ButtonIndex == MouseButton.Left)
		{
			GD.Print("Input");
			Vector2 mousePosition = eventMouseButton.Position;

			Camera3D camera = (Camera3D)GetTree().Root.FindChild("Camera3D", true, false);
			Vector3 from = camera.ProjectRayOrigin(mousePosition);
			Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 20;

			var spaceState = GetWorld3D().DirectSpaceState;
			var result = spaceState.IntersectRay(PhysicsRayQueryParameters3D.Create(from, to));
			GD.Print(result);

			if (result.Count > 0)
			{
				GD.Print("Count > 0");
				if (result.TryGetValue("collider", out var colliderVariant))
				{
					Node collider = (Node)colliderVariant;
					if (collider is Ore miningable)
						miningable.Mine(1);

					if (collider is Machine machine)
						machine.OnInteract();

					//if (collider.HasMethod("SomeMethod"))
					//{
					//	collider.Call("SomeMethod");
					//}
				}
			}
		}
	}
}
