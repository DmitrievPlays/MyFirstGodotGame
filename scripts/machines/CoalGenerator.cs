using System;
using System.Threading.Tasks;
using Godot;

public partial class CoalGenerator : Machine
{
    [Export]
    public string inventoryTemporaryName;

    public CoalGeneratorInventory Inventory;
    private PackedScene _screenPrefab;
    private CustomInventoryScreen _screen;

    private GpuParticles3D _particles;
    private InventoryHolderScreen _inventoryHolder;
    private AudioStreamPlayer3D _audio;

    private float _burnTime = 10;
    private bool _isBurning;

    public override void _Ready()
    {
        var resourceManager = ResourceManager.Instance;
        var inventoryManager = InventoryManager.Instance;
        Inventory = inventoryManager.GetBuildingInventory(inventoryTemporaryName) as CoalGeneratorInventory;

        _screenPrefab = ResourceLoader.Load<PackedScene>("res://custom_screen.tscn");

        _particles = GetNode<GpuParticles3D>("Smoke");
        var manager = ResourceManager.Instance;

        _inventoryHolder = GetNode<InventoryHolderScreen>("/root/MainScene/MainUI/InventoryHolderScreen");
        _audio = GetNode<AudioStreamPlayer3D>("Audio");

        Inventory.OnInventoryChanged += OnInventoryChanged;
    }

    public override void OnInteract()
    {
        var generatorScreen = _screenPrefab.Instantiate<CustomInventoryScreen>();
        var playerScreen = _screenPrefab.Instantiate<CustomInventoryScreen>();

        if (_inventoryHolder.Visible)
        {
            _inventoryHolder.HideScreen();
            return;
        }

        generatorScreen.ScreenName = "Coal Generator";
        generatorScreen.Inventory = Inventory;

        playerScreen.ScreenName = "Player";
        playerScreen.Inventory = InventoryManager.Instance.GetPlayerInventory("Current");

        _inventoryHolder.ShowScreen([generatorScreen, playerScreen], this);
    }

    public void OnInventoryChanged(object sender, EventArgs e)
    {
        if (!_isBurning)
            StartBurning();
    }

    private void StartBurning()
    {
        Slot slot = Inventory.GetSlotByIndex(1);
        if (slot.Resource is Coal && slot.ItemAmount > 0)
        {
            _isBurning = true;
            _particles.CallDeferred("set_emitting", true);

            var audioStream = (AudioStream)ResourceLoader.Load(@$"res://sounds/coal_generator/coal_generator_start.mp3");
            _audio.Stream = audioStream;
            _audio.Play();
            _audio.Finished += PlayContinuously;

            UpdateState();
        }
    }

    private void StopBurning()
    {
        var audioStream = (AudioStream)ResourceLoader.Load(@$"res://sounds/coal_generator/coal_generator_stop.mp3");
        _audio.Stream = audioStream;
        _audio.Play();
    }

    private void PlayContinuously()
    {
        _audio.Finished -= PlayContinuously;
        var audioStream = (AudioStream)ResourceLoader.Load(@$"res://sounds/coal_generator/coal_generator_going.mp3");
        _audio.Stream = audioStream;
        _audio.Play();
    }

    public async void UpdateState()
    {
        Slot slot = Inventory.GetSlotByIndex(1);
        GD.Print("Generator coal: " + slot.ItemAmount);

        if (slot.ItemAmount == 0)
        {
            _isBurning = false;
            _particles.CallDeferred("set_emitting", false);
            StopBurning();
            return;
        }

        if (slot.Resource is Coal && slot.ItemAmount > 0)
        {
            Inventory.RemoveItem(slot.Resource, 1);
            await Task.Delay((int)(_burnTime * 1000));
            UpdateState();
        }
    }
}
