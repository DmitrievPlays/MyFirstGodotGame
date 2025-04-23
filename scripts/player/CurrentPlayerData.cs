using Godot;

public partial class CurrentPlayerData : Node
{
    // Define a delegate (signature of the event handler)
    public delegate void MyEventHandler();

    // Define the event using the delegate
    public event MyEventHandler MyEvent;

    public static CurrentPlayerData Instance { get; private set; }

    private int _health = 100;
    private int _food = 50;
    private int _water = 60;

    public int Health
    { get { return _health; } set { _health = value; PropertyChanged(); } }

    public int Food
    { get { return _food; } set { _food = value; PropertyChanged(); } }

    public int Water
    { get { return _water; } set { _water = value; PropertyChanged(); } }

    public override void _Ready()
    {
        Instance = this;
    }

    public void PropertyChanged()
    {
        MyEvent.Invoke();
    }
}
