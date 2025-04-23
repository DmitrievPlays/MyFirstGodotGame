using Godot;
using Quaternion = Godot.Quaternion;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

public partial class PlayerControl : CharacterBody3D
{
    private bool addedHead = false;

    private float _fallStartHeight;
    private bool _isFalling = false;

    public override void _EnterTree()
    {
        if (FindChild("Head") is not null)
        {
            addedHead = true;

            if (Engine.IsEditorHint() && !addedHead)
                addedHead = true;
        }
    }

    // PLAYER MOVEMENT SCRIPT //

    #region PLAYER_CONTROL_PROPERTIES

    [ExportCategory("Mouse Capture")]
    [Export]
    private bool CAPTURE_ON_START = true;

    [ExportCategory("Movement")]
    [ExportSubgroup("Settings")]
    [Export]
    public float SPEED = 5.0f;

    [Export]
    private float SPRINT_ACCEL = 3.5f;

    [Export]
    private float ACCEL = 50.0f;

    [Export]
    private float IN_AIR_SPEED = 3.0f;

    [Export]
    private float IN_AIR_ACCEL = 5.0f;

    [Export]
    private float JUMP_VELOCITY = 4.5f;

    [Export]
    private float FALL_DAMAGE_THRESHOLD = 2f;

    [Export]
    private float FALL_DAMAGE_MULTIPLIER = 2f;

    [ExportSubgroup("Head Bob")]
    [Export]
    private bool HEAD_BOB = true;

    [Export]
    private float HEAD_BOB_FREQUENCY = 0.3f;

    [Export]
    private float HEAD_BOB_AMPLITUDE = 0.01f;

    [ExportSubgroup("Clamp Head Rotation")]
    [Export]
    private bool CLAMP_HEAD_ROTATION = true;

    [Export]
    private float CLAMP_HEAD_ROTATION_MIN = -90;

    [Export]
    private float CLAMP_HEAD_ROTATION_MAX = 90;

    [ExportCategory("Key Binds")]
    [ExportSubgroup("Mouse")]
    [Export]
    private bool MOUSE_ACCEL = true;

    [Export]
    private float KEY_BIND_MOUSE_SENSE = 0.005f;

    [Export]
    private float KEY_BIND_MOUSE_ACCEL = 50;

    [ExportSubgroup("Movement")]
    [Export]
    private string KEY_BIND_UP = "move_forward";

    [Export]
    private string KEY_BIND_LEFT = "move_left";

    [Export]
    private string KEY_BIND_RIGHT = "move_right";

    [Export]
    private string KEY_BIND_DOWN = "move_backward";

    [Export]
    private string KEY_BIND_JUMP = "jump";

    [Export]
    private string KEY_BIND_SPRINT = "sprint";

    [ExportCategory("Advanced")]
    [Export]
    private bool UPDATE_PLAYER_ON_PHYS_STEP = true; // When check player is moved and rotated in _physics_process (fixed fps)

    // Otherwise player is updated in _process (uncapped)

    // Get the gravity from the project settings to be synced with RigidBody nodes.

    private double gravity = (double)ProjectSettings.GetSetting("physics/3d/default_gravity");
    // To keep track of current speed and acceleration

    private float speed;

    private float accel;

    // Used when lerping rotation to reduce stuttering when moving the mouse

    private float rotation_target_player;

    private float rotation_target_head;

    // Used when bobing head

    private Vector3 head_start_pos;

    // Current player tick, used in head bob calculation
    private int tick = 0;

    #endregion PLAYER_CONTROL_PROPERTIES

    public override void _Ready()
    {
        speed = SPEED;
        accel = ACCEL;

        if (Engine.IsEditorHint())
            return;

        // Capture mouse if set to true
        if (CAPTURE_ON_START)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;

            head_start_pos = ((Node3D)GetNode("Head")).Position;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint())
            return;

        // Increment player tick, used in head bob motion
        tick += 1;

        if (UPDATE_PLAYER_ON_PHYS_STEP)
        {
            move_player(delta);

            rotate_player(delta);
        }

        if (HEAD_BOB)
        {
            if (!Velocity.IsZeroApprox() && IsOnFloor())
            {
                head_bob_motion();
            }

            reset_head_bob(delta);
        }
        // Only move head when on the floor and moving
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;

        if (!UPDATE_PLAYER_ON_PHYS_STEP)
        {
            move_player(delta);

            rotate_player(delta);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Engine.IsEditorHint())
            return;

        // Listen for mouse movement and check if mouse is captured

        if (@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            set_rotation_target((@event as InputEventMouseMotion).Relative);
        }

        void set_rotation_target(Vector2 mouse_motion)
        {
            // Add player target to the mouse -x input

            rotation_target_player += -mouse_motion.X * KEY_BIND_MOUSE_SENSE;
            // Add head target to the mouse -y input

            rotation_target_head += -mouse_motion.Y * KEY_BIND_MOUSE_SENSE;
            // Clamp rotation
            if (CLAMP_HEAD_ROTATION)
                rotation_target_head = Mathf.Clamp(rotation_target_head, Mathf.DegToRad(CLAMP_HEAD_ROTATION_MIN), Mathf.DegToRad(CLAMP_HEAD_ROTATION_MAX));
        }
    }

    private void rotate_player(double delta)
    {
        if (MOUSE_ACCEL)
        {
            // Shperical lerp between player rotation and target

            Quaternion = Quaternion.Slerp(new Quaternion(Vector3.Up, rotation_target_player), (float)(KEY_BIND_MOUSE_ACCEL * delta));

            // Same again for head

            ((Node3D)FindChild("Head")).Quaternion = ((Node3D)FindChild("Head")).Quaternion.Slerp(new Quaternion(Vector3.Right, rotation_target_head), (float)(KEY_BIND_MOUSE_ACCEL * delta));
        }
        else
        {
            // If mouse accel is turned off, simply set to target

            Quaternion = new Quaternion(Vector3.Up, rotation_target_player);

            ((Node3D)FindChild("Head")).Quaternion = new Quaternion(Vector3.Right, rotation_target_head);
        }
    }

    private float fallSpeed;

    private void move_player(double delta)
    {
        // Check if not on floor

        if (!IsOnFloor())
        {
            _isFalling = true;
            // Reduce speed and accel

            speed = IN_AIR_SPEED;
            accel = IN_AIR_ACCEL;
            // Add the gravity

            float temp_move = Velocity.Y;
            Velocity = new Vector3(Velocity.X, temp_move -= (float)(gravity * delta), Velocity.Z);
        }
        else
        {
            // Set speed and accel to defualt
            speed = SPEED;
            accel = ACCEL;

            if (Mathf.Abs(fallSpeed) > FALL_DAMAGE_THRESHOLD)
            {
                ApplyDamage(Mathf.Abs(fallSpeed));
            }
        }

        float resultSpeed = SPEED;
        // Handle Jump.

        if (Input.IsActionJustPressed(KEY_BIND_JUMP) && IsOnFloor())
            Velocity = new Vector3(Velocity.X, JUMP_VELOCITY, Velocity.Z);

        if (Input.IsActionPressed(KEY_BIND_SPRINT) && IsOnFloor())
            resultSpeed += SPRINT_ACCEL;

        // Get the input direction and handle the movement/deceleration.

        Vector2 input_dir = Input.GetVector(KEY_BIND_LEFT, KEY_BIND_RIGHT, KEY_BIND_UP, KEY_BIND_DOWN);

        var direction = (Transform.Basis * new Vector3(input_dir.X, 0, input_dir.Y)).Normalized();

        Velocity = new Vector3((float)Mathf.MoveToward(Velocity.X, direction.X * resultSpeed, accel * delta), Velocity.Y, Velocity.Z);
        Velocity = new Vector3(Velocity.X, Velocity.Y, (float)Mathf.MoveToward(Velocity.Z, direction.Z * resultSpeed, accel * delta));

        fallSpeed = Velocity.Y;
        MoveAndSlide();
    }

    private void head_bob_motion()
    {
        Vector3 pos = Vector3.Zero;
        pos.Y += Mathf.Sin(tick * HEAD_BOB_FREQUENCY) * HEAD_BOB_AMPLITUDE;

        pos.X += Mathf.Cos(tick * HEAD_BOB_FREQUENCY / 2) * HEAD_BOB_AMPLITUDE * 2;

        ((Node3D)FindChild("Head")).Position += pos;
    }

    private void reset_head_bob(double delta)
    {
        // Lerp back to the staring position
        if (((Node3D)FindChild("Head")).Position == head_start_pos)
            ((Node3D)FindChild("Head")).Position = new Vector3().Lerp(head_start_pos, (float)(2 * (1 / HEAD_BOB_FREQUENCY) * delta));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("pause"))
            PauseHandler.Instance.OpenPauseScreen(GetTree().Root);
    }

    private void ApplyDamage(float fallSpeed)
    {
        float damage = (fallSpeed - FALL_DAMAGE_THRESHOLD) * FALL_DAMAGE_MULTIPLIER;

        if (damage > 0)
            CurrentPlayerData.Instance.Health -= (int)damage;
    }
}
