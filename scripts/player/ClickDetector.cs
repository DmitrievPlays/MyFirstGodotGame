using Godot;

public partial class ClickDetector : Camera3D
{
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left } eventMouseButton)
		{
			Vector2 mousePosition = eventMouseButton.Position;

			Camera3D camera = (Camera3D)GetTree().Root.FindChild("Camera3D", true, false);
			Vector3 from = camera.ProjectRayOrigin(mousePosition);
			Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 20;

			var spaceState = GetWorld3D().DirectSpaceState;
			var result = spaceState.IntersectRay(PhysicsRayQueryParameters3D.Create(from, to));

			if (result.Count > 0)
			{
				if (result.TryGetValue("collider", out var colliderVariant))
				{
					Node collider = (Node)colliderVariant;
					if (collider is Ore miningable)
						miningable.Mine(1);

					if (collider is Machine machine)
						machine.OnInteract();
					
					// if (collider is IInteractable interactable)
					// 	interactable.Interact();

					//if (collider.HasMethod("SomeMethod"))
					//{
					//	collider.Call("SomeMethod");
					//}
				}
			}
		}
	}
}
