using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
	public PlayerInventory Inventory;
	private PlayerInventoryScreen screen;

	[Export]
	public long player = 1;

	private MultiplayerSynchronizer input;

	[ExportCategory("Character")]
	[Export]
	private float baseSpeed = 3.0f;

	[Export]
	private float sprintSpeed = 6.0f;

	[Export]
	private float crouchSpeed = 1.0f;

	[Export]
	private float acceleration = 10.0f;

	[Export]
	private float jumpVelocity = 4.5f;

	[Export]
	private float mouseSensitivity = 0.005f;

	[Export]
	private bool immobile = false;

	[Export]
	private Vector3 initial_facing_direction = Vector3.Zero;

	[ExportGroup("Nodes")]
	[Export]
	private Node3D HEAD;

	[Export]
	private Camera3D CAMERA;

	[Export]
	private AnimationPlayer HEADBOB_ANIMATION;

	[Export]
	private AnimationPlayer JUMP_ANIMATION;

	[Export]
	private AnimationPlayer CROUCH_ANIMATION;

	[Export]
	private CollisionShape3D COLLISION_MESH;

	[Export]
	private ShapeCast3D CEILING_DETECTION;

	[ExportGroup("Controls")]
	[Export]
	private String JUMP = "jump";

	[Export]
	private String LEFT = "move_left";

	[Export]
	private String RIGHT = "move_right";

	[Export]
	private String FORWARD = "move_forward";

	[Export]
	private String BACKWARD = "move_backward";

	[Export]
	private String PAUSE = "pause";

	[Export]
	private String CROUCH = "crouch";

	[Export]
	private String SPRINT = "sprint";

	[ExportGroup("Feature Settings")]
	[Export]
	private bool jumpingEnabled = true;

	[Export]
	private bool inAirMomentum = true;

	[Export]
	private bool motionSmoothing = true;

	[Export]
	private bool sprintEnabled = true;

	[Export]
	private bool crouchEnabled = true;

	[Export(PropertyHint.Enum, "Hold to Crouch,Toggle Crouch")]
	public int crouchMode = 0;

	[Export(PropertyHint.Enum, "Hold to Sprint,Toggle Sprint")]
	public int sprintMode = 0;

	[Export]
	private bool dynamicFOV = true;

	[Export]
	private bool continuousJumping = true;

	[Export]
	private bool viewBobbing = true;

	[Export]
	private bool jumpAnimation = true;

	// Member variables
	private float speed;

	private float currentSpeed = 0.0f;

	// States: normal, crouching, sprinting
	private String state = "normal";

	private bool lowCeiling = false; // This is for when the ceiling is too low and the player needs to crouch.
	private bool wasOnFloor = true;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 9.8f;//ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public double timeInAir;

	[Export]
	public int health = 100;

	[Export]
	public int food = 100;

	[Export]
	public int water = 100;

	public VBoxContainer playerStats;

	public override async void _Ready()
	{
		playerStats = GetTree().Root.GetNode<VBoxContainer>("/root/MainScene/MainUI/PlayerStats/MarginContainer/VBoxContainer");

		speed = baseSpeed;
		Input.MouseMode = Input.MouseModeEnum.Captured;

		screen = GetNode<PlayerInventoryScreen>("/root/MainScene/MainUI/InventoryScreen");
		var resourceManager = ResourceManager.Instance;
		var inventoryManager = PlayerInventories.Instance;

		Inventory = inventoryManager.GetPlayerInventory("Player01") as PlayerInventory;

		Inventory.AddItem(await resourceManager.GetResource(OriginalResources.ResourceType.GOLD), 2);
		Inventory.AddItem(await resourceManager.GetResource(OriginalResources.ResourceType.IRON), 28);
		Inventory.AddItem(await resourceManager.GetResource(OriginalResources.ResourceType.GOLD), 9);
		Inventory.AddItem(await resourceManager.GetResource(ModResources.ResourceType.FLASHLIGHT), 1);
		Inventory.AddItem(await resourceManager.GetResource(OriginalResources.ResourceType.GOLD), 34);
		Inventory.AddItem(await resourceManager.GetResource(ModResources.ResourceType.AXE), 2);

		Inventory.RemoveItem(await resourceManager.GetResource(OriginalResources.ResourceType.GOLD), 30);

		//((MultiplayerSynchronizer)FindChild("PlayerInput", true, false)).SetMultiplayerAuthority((int)player);
		input = ((MultiplayerSynchronizer)FindChild("PlayerInput"));

		if (player == Multiplayer.GetUniqueId())
		{
			//((Camera3D)GetTree().Root.FindChild("Camera3D", true, false)).Current = true;
		}

		// Set the camera rotation to whatever initial_facing_direction is, as long as it's not Vector3.zero
		if (!initial_facing_direction.Equals(Vector3.Zero))
		{
			HEAD.RotationDegrees = initial_facing_direction;
		}

		// HEADBOB_ANIMATION.Play("RESET");
		// JUMP_ANIMATION.Play("RESET");
		// CROUCH_ANIMATION.Play("RESET");
	}

	public override void _Process(double delta)
	{
		playerStats.GetNode<TextureProgressBar>("health/HBoxContainer/MarginContainer/value").Value = health;
		playerStats.GetNode<TextureProgressBar>("water/HBoxContainer/MarginContainer/value").Value = water;
		playerStats.GetNode<TextureProgressBar>("food/HBoxContainer/MarginContainer/value").Value = food;
	}

	public override void _PhysicsProcess(double delta)
	{
		currentSpeed = Vector3.Zero.DistanceTo(GetRealVelocity());

		Vector3 cv = GetRealVelocity();

		if (!IsOnFloor() && !IsOnWall())
			timeInAir += delta;

		// gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
		HandleGravityAndJumping(delta);

		Vector2 inputDir = immobile ? Vector2.Zero : Input.GetVector(LEFT, RIGHT, FORWARD, BACKWARD);
		HandleMovement(delta, inputDir);

		lowCeiling = CEILING_DETECTION.IsColliding();
		handleState(inputDir != Vector2.Zero);

		if (dynamicFOV) { updateCameraFOV(); }

		if (viewBobbing) { headbobAnimation(inputDir != Vector2.Zero); }

		if (jumpAnimation)
		{
			if (!wasOnFloor && IsOnFloor()) // Just Landed
			{
				if (timeInAir > 2)
					health -= (int)(timeInAir * 30);
				timeInAir = 0;
				//JUMP_ANIMATION.Play((GD.Randi() % 2) == 1 ? "land_left" : "land_right") ;
			}
			wasOnFloor = IsOnFloor(); //This must always be at the end of physics_process
		}
	}

	private void HandleGravityAndJumping(double delta)
	{
		Vector3 currentVelocity = Velocity;
		if (!IsOnFloor())
		{
			currentVelocity.Y -= (float)(gravity * delta);
		}
		else if (jumpingEnabled)
		{
			if (continuousJumping ? Input.IsActionPressed(JUMP) : Input.IsActionJustPressed(JUMP))
			{
				if (IsOnFloor() && !lowCeiling)
				{
					if (JUMP_ANIMATION != null)
					{
						//JUMP_ANIMATION.Play("jump") ;
					}
					currentVelocity.Y += jumpVelocity;
				}
			}
		}
		Velocity = currentVelocity;
	}

	private void HandleMovement(double delta, Vector2 inputDir)
	{
		Vector2 direction2D = inputDir.Rotated(-HEAD.Rotation.Y);
		Vector3 direction = new Vector3(direction2D.X, 0, direction2D.Y);
		direction = direction.Normalized(); // ?
		MoveAndSlide();

		if (!inAirMomentum || IsOnFloor())
		{
			Vector3 currentVelocity = Vector3.Zero;
			currentVelocity.X = motionSmoothing ? Mathf.Lerp(Velocity.X, direction.X * speed, (float)(acceleration * delta)) : direction.X * speed;
			currentVelocity.Z = motionSmoothing ? Mathf.Lerp(Velocity.Z, direction.Z * speed, (float)(acceleration * delta)) : direction.Z * speed;
			Velocity = currentVelocity;
		}
	}

	private void handleState(bool moving)
	{
		if (sprintEnabled)
		{
			if (sprintMode == 0)
			{
				if (Input.IsActionPressed(SPRINT) && state != "crouching")
				{
					if (moving)
					{
						if (state != "sprinting")
						{
							enterSprintState();
						}
					}
					else
					{
						if (state == "sprinting")
						{
							enterNormalState();
						}
					}
				}
				else if (state == "sprinting")
				{
					enterNormalState();
				}
			}
			else if (sprintMode == 1)
			{
				if (moving)
				{
					if (Input.IsActionPressed(SPRINT) && state == "normal")
					{
						enterSprintState();
					}
					if (Input.IsActionJustPressed(SPRINT))
					{
						switch (state)
						{
							case "normal":
								enterSprintState();
								break;

							case "sprinting":
							default:
								enterNormalState();
								break;
						}
					}
				}
				else if (state == "sprinting")
				{
					enterNormalState();
				}
			}
		}

		if (crouchEnabled)
		{
			if (crouchMode == 0)
			{
				if (Input.IsActionPressed(CROUCH) && state != "sprinting")
				{
					if (state != "crouching")
					{
						enterCrouchState();
					}
				}
				else if (state == "crouching" && !CEILING_DETECTION.IsColliding())
				{
					enterNormalState();
				}
			}
			else if (crouchMode == 1)
			{
				if (Input.IsActionJustPressed(CROUCH))
				{
					switch (state)
					{
						case "normal":
							enterCrouchState();
							break;

						case "crouching":
						default:
							if (!CEILING_DETECTION.IsColliding())
							{
								enterNormalState();
							}
							break;
					}
				}
			}
		}
	}

	private void enterNormalState()
	{
		String previousState = state;
		if (previousState == "crouching")
		{
			//CROUCH_ANIMATION.PlayBackwards("crouch") ;
		}
		state = "normal";
		speed = baseSpeed;
	}

	private void enterSprintState()
	{
		String previousState = state;
		if (previousState == "crouching")
		{
			//CROUCH_ANIMATION.PlayBackwards("crouch") ;
		}
		state = "sprinting";
		speed = sprintSpeed;
	}

	private void enterCrouchState()
	{
		String previousState = state;
		state = "crouching";
		speed = crouchSpeed;
		//CROUCH_ANIMATION.Play("crouch") ;
	}

	private void updateCameraFOV()
	{
		CAMERA.Fov = Mathf.Lerp(CAMERA.Fov, state == "sprinting" ? 85 : 75, 0.3f);
	}

	private void headbobAnimation(bool moving)
	{
		if (moving && IsOnFloor())
		{
			String useHeadbobAnimation = (state == "normal" || state == "crouching") ? "walk" : "sprint";
			//bool wasPlaying = HEADBOB_ANIMATION.CurrentAnimation == useHeadbobAnimation ;

			//HEADBOB_ANIMATION.Play(useHeadbobAnimation, 0.25f) ;
			//HEADBOB_ANIMATION.SpeedScale = (currentSpeed / baseSpeed) * 1.75f ;
			//if (!wasPlaying) { HEADBOB_ANIMATION.Seek((double)(GD.Randi() % 2)); }
		}
		else
		{
			//HEADBOB_ANIMATION.Play("RESET", 0.25) ;
			//HEADBOB_ANIMATION.SpeedScale = 1 ;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			Input.MouseMode = (Input.MouseMode == Input.MouseModeEnum.Captured) ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
		}

		if (@event.IsActionPressed("debug"))
		{
			var temp = GetTree().Root.GetNode<Control>("/root/MainScene/MainUI/Debug");
			temp.Visible = !temp.Visible;
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			InputEventMouseMotion iemm = (InputEventMouseMotion)@event;
			Vector3 currentRotation = HEAD.Rotation;
			currentRotation.Y -= iemm.Relative.X * mouseSensitivity;
			currentRotation.X -= iemm.Relative.Y * mouseSensitivity;
			HEAD.Rotation = currentRotation;
		}

		if (Input.IsActionJustPressed("inventory"))
		{
			screen.ShowScreen(Inventory, this.HEAD);
		}
	}
}