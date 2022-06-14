namespace Flow.Launcher.Plugin.EagleCool
{
#pragma warning disable CS1591
    public class FlowLauncherApiMessenger : IMessenger
    {
        readonly IPublicAPI _api;

        public FlowLauncherApiMessenger(IPublicAPI api) => _api = api;
        public void Say(string title, string details) => _api.ShowMsg(title,details);
    }
}