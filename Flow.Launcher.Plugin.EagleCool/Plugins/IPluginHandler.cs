using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.EagleCool
{
    interface IPluginHandler
    {
        Task InitAsync(PluginInitContext context);

        Task<List<Result>> QueryAsync(Query query, CancellationToken token);
        void Dispose();
        List<Result> ResultSelected(Result selected);
    }
}