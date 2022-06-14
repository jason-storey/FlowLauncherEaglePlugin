using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eagle;
using static Flow.Launcher.Plugin.EagleCool.ResultFactory;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.EagleCool
{
    public class EagleFancierPlugin : BasePlugin
    {
        public override async Task InitAsync(PluginInitContext context)
        {
            await base.InitAsync(context);
            _messenger = new FlowLauncherApiMessenger(Api);
            _eagle = new EagleService();
            _runner = new EagleQueryRunner(_eagle, Api,_messenger);
            _library = await _eagle.GetLibrarySummary(CancellationToken.None);
        }
        
        protected override async Task<List<Result>> GetResults(Query query, CancellationToken token)
        {
            try
            {
                return await _runner.Execute(query, token);
            }
            catch (Exception ex)
            {
                return FromException(ex);
            }
        }

        public override List<Result> ResultSelected(Result selected)
        {
            if (selected.ContextData is TagGroupContext tg) return HandleTagGroupSelection(tg,_eagle,_messenger);
            return base.ResultSelected(selected);
        }

        #region Plumbing

        LibrarySummary _library;
        EagleService _eagle;
        QueryLookup _queries;
        IMessenger _messenger;
        EagleQueryRunner _runner;

        #endregion
    }
}