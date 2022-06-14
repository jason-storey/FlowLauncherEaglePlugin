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
        Exception _lastCaptured;
        public override async Task InitAsync(PluginInitContext context)
        {
            try
            {
                await base.InitAsync(context);
                _eagle = new EagleService();
                _messenger = new FlowLauncherApiMessenger(context.API);
                _library = new LibrarySummary(); 
                _runner = new EagleQueryRunner(_eagle,context.API, _messenger, _library);
            
            }
            catch (Exception ex)
            {
                _lastCaptured = ex;
            }
        }

        protected override async Task<List<Result>> GetResults(Query query, CancellationToken token)
        {
            try
            {
                if (_runner == null) return new Results(FromException(_lastCaptured)) { "Failed to load Runner", };
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
            if (selected.ContextData is FolderContext f) return HandleFolderSelection(f, _eagle, _messenger);
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