using Godot;
using System;
using System.Timers;

public partial class Furnace : Machine
{
    public FurnaceInventory Inventory;
    private PackedScene screenPrefab;
    private CustomInventoryScreen screen;

    [Export]
    public string inventoryTemporaryName;

    private GpuParticles3D particles;
    private InventoryHolderScreen inventoryHolder;

    private int fireSlot = 2;
    private int meltSlot = 1;
    private int resultSlot = 3;

    private float meltTime = 2;
    private bool isBurning;

    private System.Timers.Timer timer;

    public override async void _Ready()
    {
        var resourceManager = ResourceManager.Instance;
        var inventoryManager = InventoryManager.Instance;
        Inventory = inventoryManager.GetBuildingInventory(inventoryTemporaryName) as FurnaceInventory;

        screenPrefab = ResourceLoader.Load<PackedScene>("res://custom_screen.tscn");

        particles = GetNode<GpuParticles3D>("Smoke");
        var manager = ResourceManager.Instance;

        inventoryHolder = GetNode<InventoryHolderScreen>("/root/MainScene/MainUI/InventoryHolderScreen");

        Inventory.OnInventoryChanged += OnInventoryChanged;

        Inventory.AddItem(await manager.GetResource(OriginalResources.ResourceType.GOLD), 3);
        Inventory.AddItem(await manager.GetResource(OriginalResources.ResourceType.COAL), 10, 2);
    }

    public void OnInventoryChanged(object sender, EventArgs e)
    {
        GD.Print("Inventory changed");

        if (Inventory.GetItems()[fireSlot].Resource is Coal && Inventory.GetItems()[fireSlot].ItemAmount > 0)
        {
            if (!isBurning)
                MakeThisShitBurn();
            isBurning = true;
            particles.CallDeferred("set_emitting", true);
        }
        GD.Print(Inventory.GetItems()[fireSlot].ItemAmount);
        if (Inventory.GetItems()[fireSlot].ItemAmount == 0)
        {
            isBurning = false;
            particles.CallDeferred("set_emitting", false);

            if (timer != null)
                StopThisShipFromBurning();
        }
    }

    private void MakeThisShitBurn()
    {
        timer = new System.Timers.Timer();

        timer.Interval = meltTime * 1000;
        timer.Start();
        timer.Elapsed += UpdateState;
    }

    private void StopThisShipFromBurning()
    {
        timer.Dispose();
        timer.Stop();
    }

    public void UpdateState(object sender, ElapsedEventArgs e)
    {
        Slot slot = Inventory.GetSlotByIndex(fireSlot);

        if (slot.Resource is not null)
            Inventory.RemoveItem(slot.Resource, 1);
    }

    public override void OnInteract()
    {
        var furnaceScreen = screenPrefab.Instantiate<CustomInventoryScreen>();
        var playerScreen = screenPrefab.Instantiate<CustomInventoryScreen>();

        if (inventoryHolder.Visible)
        {
            inventoryHolder.HideScreen();
            return;
        }

        furnaceScreen.ScreenName = "Furnace";
        furnaceScreen.Inventory = Inventory;

        playerScreen.ScreenName = "PLayer";
        playerScreen.Inventory = InventoryManager.Instance.GetPlayerInventory("Current");

        inventoryHolder.ShowScreen([furnaceScreen, playerScreen], this);
    }
}
