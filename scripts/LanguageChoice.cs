using Godot;

public partial class LanguageChoice : PanelContainer
{
    [Export]
    public ButtonGroup ButtonGroup;

    [Export]
    public string[] ButtonLanguages;

    public override void _Ready()
    {
        var language = SettingsHandler.Instance.GetSetting("language");
        TranslationServer.SetLocale(language);

        foreach (var item in ButtonGroup.GetButtons())
        {
            item.Connect("pressed", Callable.From(() => ChangeLanguage(item.GetIndex())));
            if (item.GetIndex() == Array.FindIndex(ButtonLanguages, row => row.Contains(language)))
            {
                item.ButtonPressed = true;
            }
        }
    }

    public void ChangeLanguage(int id)
    {
        if (id == 0)
        {
            TranslationServer.SetLocale("ru");
            SettingsHandler.Instance.PutSetting("language", "ru");
            GD.Print("pressed 0");
        }
        else if (id == 1)
        {
            TranslationServer.SetLocale("en");
            SettingsHandler.Instance.PutSetting("language", "en");
            GD.Print("pressed 1");
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsKeyPressed(Key.Escape))
            Hide();
    }

    public void InputEvent(Node node, InputEvent @event)
    {
        GD.Print("pressed");
        Hide();
        base._Input(@event);
    }
}
