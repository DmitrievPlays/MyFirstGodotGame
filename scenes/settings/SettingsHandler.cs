using Godot;
using Newtonsoft.Json;

public class Setting
{
    public Setting(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; set; }
    public string Value { get; set; }
}

public partial class SettingsHandler : Node
{
    private string _settingsDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\somefactory_data";
    private string FILE_NAME = "settings.json";
    private string SETTINGS_FILE_PATH;

    private List<Setting> _settings = new List<Setting>() { };

    private PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://scenes/settings/settings_screen.tscn");
    private SettingsScreen SettingsScreen;

    private bool isAdded = false;

    public static SettingsHandler Instance { get; private set; }

    public override void _Ready()
    {
        SettingsScreen = (SettingsScreen)packedScene.Instantiate();
        Instance = this;

        SETTINGS_FILE_PATH = @$"{_settingsDirectory}/{FILE_NAME}";

        Directory.CreateDirectory(_settingsDirectory);
        LoadSettingsFromFile();

        string json = JsonConvert.SerializeObject(_settings?.ToArray());
    }

    public void PutSetting(string key, string value)
    {
        if (_settings is null)
            _settings = new List<Setting>();

        if (_settings.Any(x => x.Name == key))
            _settings.Where(x => x.Name == key).First().Value = value;
        else _settings.Add(new Setting(key, value));
    }

    public string GetSetting(string key)
    {
        if (_settings is null)
            _settings = new List<Setting>();

        if (_settings.Any(x => x.Name == key))
            return _settings.Where(x => x.Name == key).First().Value;
        else return "";
    }

    public void LoadSettingsFromFile()
    {
        if (!File.Exists(SETTINGS_FILE_PATH))
            File.Create(SETTINGS_FILE_PATH);

        string jsonString = File.ReadAllText(SETTINGS_FILE_PATH);

        _settings = JsonConvert.DeserializeObject<List<Setting>>(jsonString);
    }

    public void ApplySettings()
    {
        if (!File.Exists(SETTINGS_FILE_PATH))
            File.Create(SETTINGS_FILE_PATH);

        string json = JsonConvert.SerializeObject(_settings, Formatting.Indented);

        File.WriteAllText(SETTINGS_FILE_PATH, json);
    }

    public string GetSettingsDirectory()
    {
        return _settingsDirectory;
    }

    public void OpenSettingsScreen(Node parent)
    {
        SettingsScreen.Show();

        if (!isAdded)
        {
            parent.AddChild(SettingsScreen);
            isAdded = true;
        }
    }
}
