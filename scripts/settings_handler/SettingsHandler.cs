using Godot;

public partial class SettingsHandler : Node
{
    private Dictionary<string, string> _settings = new Dictionary<string, string>();
    private string _settingsDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\somefactory_data";
    public static SettingsHandler Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        Directory.CreateDirectory(_settingsDirectory);
        LoadSettingsFromFile();
    }

    public void PutSetting(string key, string value)
    {
        _settings[key] = value;
        SaveSettingsToFile();
    }

    public string GetSetting(string key)
    {
        if (_settings.ContainsKey(key))
            return _settings[key];
        return null;
    }

    public void LoadSettingsFromFile()
    {
        var f = Godot.FileAccess.Open(@$"{_settingsDirectory}/settings.dat", Godot.FileAccess.ModeFlags.Read);
        if (f is null)
        {
            File.Create(@$"{_settingsDirectory}/settings.dat");
            f = Godot.FileAccess.Open(@$"{_settingsDirectory}/settings.dat", Godot.FileAccess.ModeFlags.Read);
        }

        using (StreamReader file = new StreamReader(@$"{_settingsDirectory}/settings.dat"))
        {
            string line;
            while ((line = file.ReadLine()) != null)
            {
                var key = line.Split(":")[0];
                var value = line.Split(":")[1].Replace(";", "");

                _settings.Add(key, value);
            }
        }
    }

    public void SaveSettingsToFile()
    {
        var exists = Godot.FileAccess.FileExists($@"{_settingsDirectory}/settings.dat");
        if (!exists)
            File.Create($@"{_settingsDirectory}/settings.dat");

        using (StreamWriter file = new StreamWriter($@"{_settingsDirectory}/settings.dat"))
        {
            foreach (var setting in _settings)
                file.WriteLine($@"{setting.Key}:{setting.Value};");

            file.Close();
        }
    }

    public string GetSettingsDirectory()
    {
        return _settingsDirectory;
    }
}
