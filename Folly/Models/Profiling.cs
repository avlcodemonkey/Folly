using Microsoft.AspNetCore.Http;

namespace Folly.Models;

public class Profiling
{
    private readonly ISession Session;
    public const string SettingName = "Profiling";

    public Profiling(ISession session) => Session = session;

    public bool IsEnabled => Session.GetString(SettingName).ToBool();
}
