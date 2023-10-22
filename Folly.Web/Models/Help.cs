using Folly.Extensions;

namespace Folly.Models;

public sealed class Help {
    private readonly ISession _Session;
    public const string SettingName = "ContextHelp";

    public Help(ISession session) => _Session = session;

    public bool IsEnabled => _Session.GetString(SettingName).ToBool();
}
