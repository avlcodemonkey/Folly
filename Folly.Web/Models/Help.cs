namespace Folly.Models;

public sealed class Help {
    private readonly ISession Session;
    public const string SettingName = "ContextHelp";

    public Help(ISession session) => Session = session;

    public bool IsEnabled => Session.GetString(SettingName).ToBool();
}